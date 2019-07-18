using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using static LGThingApi.Structures.Translate;
using Newtonsoft.Json;
using System.Linq;
using LGThingApi.Structures;

namespace LGThingApi
{
    public static class Communication
    {
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
                        throw new LGExceptions.NotLoggedInException();
                    case "0106":
                        throw new LGExceptions.NotConnectedException();
                    default:
                        throw new LGExceptions.ApiException(dataReturn.ReturnMsg);
                }
            communicationClient.DefaultRequestHeaders.Clear();
            return dataReturn;
        }

        public static class LGGateway
        {
            static string Gateway_Url => "https://kic.lgthinq.com:46030/api/common/gatewayUriList";
            public static LgedmRoot LgedmRoot { get; set; } = new LgedmRoot();
            public async static Task OpenConnection(string countryCode, string langCode)
            {
                LgedmRoot.ReplaceWithNewData(await Communication.Post(Gateway_Url, new LgedmRoot() { LangCode = langCode, CountryCode = countryCode }));

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
            public static void CloseConnection()
            {
                LgedmRoot = new LgedmRoot();
            }
            public static async Task GetDevices()
            {
                HttpClient communicationClient = new HttpClient();
                LgedmRoot.ReplaceWithNewData(await Post(Path.Combine(LgedmRoot.ThinqUri.ToString(), "device/deviceList"), LgedmRoot, LGOAuth.AuthorizationData.AccessToken, LgedmRoot.JsessionId));
                foreach (var item in LgedmRoot.Item)
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
            public static async Task StartMonitoring(Device deviceToMonitor)
            {
                deviceToMonitor.WorkID = Guid.NewGuid().ToString();
                LgedmRoot.ReplaceWithNewData(await Post(Path.Combine(LgedmRoot.ThinqUri.ToString(), "rti/rtiMon"), (new LgedmRoot() { Cmd = "Mon", CmdOpt = "Start", DeviceID = deviceToMonitor.DeviceID, WorkID = deviceToMonitor.WorkID }), LGOAuth.AuthorizationData.AccessToken, LgedmRoot.JsessionId));
                deviceToMonitor.WorkID = LgedmRoot.WorkID;
            }
            public static async Task<byte[]> PollMonitor(Device deviceToMonitor)
            {
                LgedmRoot.ReplaceWithNewData(await Post(Path.Combine(LgedmRoot.ThinqUri.ToString(), "rti/rtiResult"), (new LgedmRoot() { WorkList = new[] { new WorkList() { DeviceId = deviceToMonitor.DeviceID, WorkId = deviceToMonitor.WorkID } } }), LGOAuth.AuthorizationData.AccessToken, LgedmRoot.JsessionId));
                var item = (from temp in LgedmRoot.WorkList where temp.DeviceId == deviceToMonitor.DeviceID select temp).First();
                if (item.ReturnData != null)
                {
                    return System.Convert.FromBase64String(item.ReturnData);
                }
                return null;
            }
            public static async Task StopMonitoring(Device deviceToMonitor)
            {
                LgedmRoot.ReplaceWithNewData(await Post(Path.Combine(LgedmRoot.ThinqUri.ToString(), "rti/rtiMon"), (new LgedmRoot() { Cmd = "Mon", CmdOpt = "Stop", DeviceID = deviceToMonitor.DeviceID, WorkID = deviceToMonitor.WorkID }), LGOAuth.AuthorizationData.AccessToken, LgedmRoot.JsessionId));
            }
            public static class LGOAuth
            {
                public static AuthorizationStructure AuthorizationData { get; set; }
                public static string GetOauthUrl()
                {
                    if (LgedmRoot == null)
                        throw new InvalidOperationException("OpenConnection has to be called first!");

                    string url = Path.Combine(LgedmRoot.EmpUri.ToString(), "login/sign_in");
                    return ($"{url}?country={LgedmRoot.CountryCode}&language={LgedmRoot.LangCode}&svcCode=SVC202&authSvr=oauth2&client_id=LGAO221A02&division=ha&grant_type=password");
                }
                public static void AuthorizeBasedOnOauth(string response)
                {
                    if (response == null)
                        throw new InvalidDataException();
                    string access = response.Split('=')[1].Split('&')[0];
                    string refresh = response.Split('=')[2].Split('&')[0];

                    AuthorizationData = new AuthorizationStructure() { AccessToken = access, RefreshToken = refresh, GrantType = "refresh_token" };
                }
                public async static Task Login()
                {
                    LgedmRoot.LoginType = "EMP";
                    LgedmRoot.Token = AuthorizationData.AccessToken;
                    LgedmRoot.ReplaceWithNewData(await Post(Path.Combine(LgedmRoot.ThinqUri.ToString(), "member/login"), LgedmRoot));
                }
                public async static Task RefreshToken()//TO-DO
                {
                    return;
                    /*HttpClient communicationClient = new HttpClient();
                    string url = Path.Combine(LgedmRoot.OauthUri.ToString(), "oauth2/token");
                    string timeStamp = OAuthBase.GenerateTimeStamp();
                    url = ($"{url}?grant_type={AuthorizationData.GrantType}&refresh_token={AuthorizationData.RefreshToken}");
                    string signature = OAuthBase.GenerateOAuthSignature($"{url}\n{timeStamp}", "c053c2a6ddeb7ad97cb0eed0dcb31cf8");

                    communicationClient.DefaultRequestHeaders.Add("lgemp-x-app-key", "LGAO221A02");
                    communicationClient.DefaultRequestHeaders.Add("lgemp-x-signature", signature);
                    communicationClient.DefaultRequestHeaders.Add("lgemp-x-date", timeStamp);
                    communicationClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    var data = JsonConvert.SerializeObject(AuthorizationData);

                    var result = await communicationClient.PostAsync(url, new StringContent(data, Encoding.UTF8, "application/json"));

                    var str = await result.Content.ReadAsStringAsync();

                    var obj = JsonConvert.DeserializeObject(str);

                    communicationClient.DefaultRequestHeaders.Clear();*/
                }
            }
        }
    }
}
