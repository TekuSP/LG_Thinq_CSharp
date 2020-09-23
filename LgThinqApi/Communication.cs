using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using LGThingApi.Structures;
using LGThingApi.Extensions;
using System.Net.Http.Headers;

namespace LGThingApi
{
    /// <summary>
    /// Used to communicate with LG Servers
    /// </summary>
    public static class Communication
    {
        /// <summary>
        /// Posts request to specified LG URL
        /// </summary>
        /// <param name="url">URI to post to</param>
        /// <param name="data">Data to send</param>
        /// <param name="accessToken">User accesss token if available</param>
        /// <param name="sessionId">Session ID if available</param>
        /// <returns>Returns result data for request</returns>
        public async static Task<LgedmRoot> Post(string url, LgedmRoot data, string accessToken = null, string sessionId = null)
        {
            HttpClient communicationClient = new HttpClient();
            communicationClient.DefaultRequestHeaders.Add("x-thinq-application-key", "wideq");
            communicationClient.DefaultRequestHeaders.Add("x-thinq-security-key", "nuts_securitykey");
            communicationClient.DefaultRequestHeaders.Add("Accept", "application/json");
            if (accessToken != null)
                communicationClient.DefaultRequestHeaders.Add("x-thinq-token", accessToken);
            if (sessionId != null)
                communicationClient.DefaultRequestHeaders.Add("x-thinq-jsessionId", sessionId);

            string serialized = JsonConvert.SerializeObject(new LGReturnRoot() { LgedmRoot = data }, Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                DefaultValueHandling = DefaultValueHandling.Ignore
                            });
            HttpContent content = new StringContent(serialized, Encoding.UTF8, "application/json");

            var response = await communicationClient.PostAsync(url, content);

            var textResponse = await response.Content.ReadAsStringAsync();

            var dataReturn = JsonConvert.DeserializeObject<LGReturnRoot>(textResponse).LgedmRoot;

            if (dataReturn.ReturnCd != "0000")
                switch (dataReturn.ReturnCd)
                {
                    case "0102":
                        if (await LGGateway.LGOAuth.RefreshToken())
                        {
                            await Post(url, data, accessToken, sessionId);
                        }
                        else
                        {
                            throw new LGExceptions.NotLoggedInException();
                        }
                        break;
                    case "0106":
                        throw new LGExceptions.NotConnectedException();
                    default:
                        throw new LGExceptions.ApiException(dataReturn.ReturnMsg);
                }
            communicationClient.DefaultRequestHeaders.Clear();
            return dataReturn;
        }
        /// <summary>
        /// Low level communication
        /// </summary>
        public static class LGGateway
        {
            /// <summary>
            /// List of all gateways available from LG
            /// </summary>
            const string Gateway_Url = "https://kic.lgthinq.com:46030/api/common/gatewayUriList";
            /// <summary>
            /// Data Root, what we are working with
            /// </summary>
            public static LgedmRoot LgedmRoot { get; set; } = new LgedmRoot();
            /// <summary>
            /// Open asynchronously connection to LG and ask for country located available gateway, download all available data returned
            /// </summary>
            /// <param name="countryCode">Country code like en_US</param>
            /// <param name="langCode">Language code like en</param>
            /// <returns>Stores data into <see cref="LgedmRoot" langword="static">LgedmRoot</see></returns>
            public async static Task OpenConnection(string countryCode, string langCode)
            {
                LgedmRoot.ReplaceWithNewData(await Post(Gateway_Url, new LgedmRoot() { LangCode = langCode, CountryCode = countryCode }));

                HttpClient communicationClient = new HttpClient();
                var res = await communicationClient.GetAsync(LgedmRoot.LangPackCommonUri);
                string data = await res.Content.ReadAsStringAsync();
                LgedmRoot.CommonTranslate = JsonConvert.DeserializeObject<Translate>(data);

                if (LgedmRoot.LangPackIntroUri != null)
                {
                    res = await communicationClient.GetAsync(LgedmRoot.LangPackIntroUri);
                    data = await res.Content.ReadAsStringAsync();
                    LgedmRoot.InitTranslate = JsonConvert.DeserializeObject<Translate>(data);
                }
                else
                    LgedmRoot.InitTranslate = new Translate() { Pack = new Dictionary<string, string>() };
                communicationClient.Dispose();
            }
            /// <summary>
            /// Dispose all known data and stop all communication
            /// </summary>
            public static void CloseConnection()
            {
                LgedmRoot = new LgedmRoot();
            }
            /// <summary>
            /// Get all devices from user, WARNING, user has to be authenticated before calling this
            /// </summary>
            /// <returns>Stores devices into <see cref="LgedmRoot" langword="static">LgedmRoot</see></returns>
            public static async Task GetDevices()
            {
                HttpClient communicationClient = new HttpClient();
                LgedmRoot.ReplaceWithNewData(await Post(Path.Combine(LgedmRoot.ThinqUri.ToString(), "device/deviceList"), LgedmRoot, LGOAuth.AuthorizationData.AccessToken, LgedmRoot.JsessionId));
                foreach (var item in LgedmRoot.Item) //Replace this with Pararell?
                {
                    var res = await communicationClient.GetAsync(item.LangPackModelUri);
                    string data = await res.Content.ReadAsStringAsync();
                    item.DeviceCapabilities = JsonConvert.DeserializeObject<Translate>(data);

                    res = await communicationClient.GetAsync(item.LangPackProductTypeUri);
                    data = await res.Content.ReadAsStringAsync();
                    item.ProductTranslate = JsonConvert.DeserializeObject<Translate>(data);

                    res = await communicationClient.GetAsync(item.ModelJsonUrl);
                    data = await res.Content.ReadAsStringAsync();
                    item.ModelInfo = JsonConvert.DeserializeObject<ModelInfo>(data);
                }
                communicationClient.Dispose();
            }
            /// <summary>
            /// Start device monitoring, monitoring means checking its state
            /// </summary>
            /// <param name="deviceToMonitor">Device which we want to monitor</param>
            /// <returns>Saves WorkID (Process) back to device it was given</returns>
            public static async Task StartMonitoring(Device deviceToMonitor)
            {
                if (!string.IsNullOrWhiteSpace(deviceToMonitor.WorkID)) //Device is already monitored, restart monitor
                    await StopMonitoring(deviceToMonitor);

                deviceToMonitor.WorkID = Guid.NewGuid().ToString();
                LgedmRoot.ReplaceWithNewData(await Post(Path.Combine(LgedmRoot.ThinqUri.ToString(), "rti/rtiMon"), (new LgedmRoot() { Cmd = "Mon", CmdOpt = "Start", DeviceID = deviceToMonitor.DeviceID, WorkID = deviceToMonitor.WorkID }), LGOAuth.AuthorizationData.AccessToken, LgedmRoot.JsessionId));
                deviceToMonitor.WorkID = LgedmRoot.WorkID;
            }
            /// <summary>
            /// Get results from monitoring, this should be called repeatedly, if called too soon after starting monitor, it can return null
            /// </summary>
            /// <param name="deviceToMonitor">Device which contains WorkID (Process)</param>
            /// <returns>Returns data from LG Api, it can be bit array or JSON</returns>
            public static async Task<byte[]> PollMonitor(Device deviceToMonitor)
            {
                LgedmRoot.ReplaceWithNewData(await Post(Path.Combine(LgedmRoot.ThinqUri.ToString(), "rti/rtiResult"), (new LgedmRoot() { WorkList = new[] { new WorkList() { DeviceId = deviceToMonitor.DeviceID, WorkId = deviceToMonitor.WorkID } } }), LGOAuth.AuthorizationData.AccessToken, LgedmRoot.JsessionId));
                var item = (from temp in LgedmRoot.WorkList where temp.DeviceId == deviceToMonitor.DeviceID select temp).First();
                if (item.ReturnData != null)
                    return System.Convert.FromBase64String(item.ReturnData);
                return null;
            }
            /// <summary>
            /// Cancel and stop monitoring on specific device
            /// </summary>
            /// <param name="deviceToMonitor">Device which to cancel</param>
            /// <returns></returns>
            public static async Task StopMonitoring(Device deviceToMonitor)
            {
                LgedmRoot.ReplaceWithNewData(await Post(Path.Combine(LgedmRoot.ThinqUri.ToString(), "rti/rtiMon"), (new LgedmRoot() { Cmd = "Mon", CmdOpt = "Stop", DeviceID = deviceToMonitor.DeviceID, WorkID = deviceToMonitor.WorkID }), LGOAuth.AuthorizationData.AccessToken, LgedmRoot.JsessionId));
            }
            /// <summary>
            /// Authentication service
            /// </summary>
            public static class LGOAuth
            {
                /// <summary>
                /// Authorized data containing Access Token and Refresh Token
                /// </summary>
                public static AuthorizationStructure AuthorizationData { get; set; }
                /// <summary>
                /// Get URL for user to log-in through
                /// </summary>
                /// <returns>Returns url from LG</returns>
                public static string GetOauthUrl()
                {
                    if (LgedmRoot == null)
                        throw new InvalidOperationException("OpenConnection has to be called first!");

                    string url = Path.Combine(LgedmRoot.EmpUri.ToString(), "login/sign_in");
                    return ($"{url}?country={LgedmRoot.CountryCode}&language={LgedmRoot.LangCode}&svcCode=SVC202&authSvr=oauth2&client_id=LGAO221A02&division=ha&grant_type=password");
                }
                /// <summary>
                /// Parse access and refresh tokens based on response from Oauth Url from <see cref="GetOauthUrl" langword="static">GetOauthUrl</see>
                /// </summary>
                /// <param name="response">Response from <see cref="GetOauthUrl" langword="static">GetOauthUrl</see></param>
                public static void AuthorizeBasedOnOauth(string response)
                {
                    if (response == null)
                        throw new InvalidDataException();
                    string access = response.Split('=')[1].Split('&')[0];
                    string refresh = response.Split('=')[2].Split('&')[0];

                    AuthorizationData = new AuthorizationStructure() { AccessToken = access, RefreshToken = refresh, GrantType = "refresh_token" };
                }
                /// <summary>
                /// Parse and login refresh and access tokens based on last known access token
                /// </summary>
                public async static Task Login()
                {
                    LgedmRoot.LoginType = "EMP";
                    LgedmRoot.Token = AuthorizationData.AccessToken;
                    LgedmRoot.ReplaceWithNewData(await Post(Path.Combine(LgedmRoot.ThinqUri.ToString(), "member/login"), LgedmRoot));
                }
                /// <summary>
                /// Refreshes Access Token based on Refresh Token
                /// </summary>
                /// <returns>Returns true on success, false on not being able to refresh</returns>
                public async static Task<bool> RefreshToken()
                {
                    if (LgedmRoot == null || AuthorizationData == null || AuthorizationData.RefreshToken == null)
                        return false;
                    HttpClient communicationClient = new HttpClient();
                    string url = Path.Combine(LgedmRoot.OauthUri.ToString(), "oauth2/token");
                    string timeStamp = OAuth.GetTimestampNow();
                    string reqUrl = $"/oauth2/token?grant_type={AuthorizationData.GrantType}&refresh_token={AuthorizationData.RefreshToken}";
                    string signature = OAuth.GetOAuthSignature($"{reqUrl}\n{timeStamp}", "c053c2a6ddeb7ad97cb0eed0dcb31cf8");

                    communicationClient.DefaultRequestHeaders.Add("lgemp-x-app-key", "LGAO221A02");
                    communicationClient.DefaultRequestHeaders.Add("lgemp-x-signature", signature);
                    communicationClient.DefaultRequestHeaders.Add("lgemp-x-date", timeStamp);
                    communicationClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("grant_type", AuthorizationData.GrantType), new KeyValuePair<string, string>("refresh_token", AuthorizationData.RefreshToken) });

                    var result = await communicationClient.PostAsync(url, content);
                    var str = await result.Content.ReadAsStringAsync();
                    AuthorizationData.AccessToken = JsonConvert.DeserializeObject<AuthorizationStructure>(str).AccessToken;
                    communicationClient.Dispose();
                    return true;
                }
            }
        }
    }
}
