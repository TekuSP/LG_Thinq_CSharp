using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using LGThingApi.Extensions;

namespace LGThingApi.Structures
{
    public class LGReturnRoot
    {
        [JsonProperty("lgedmRoot")]
        public LgedmRoot LgedmRoot { get; set; }
    }
    public class LgedmRoot
    {
        [JsonProperty("returnCd")]
        public string ReturnCd { get; set; }

        [JsonProperty("returnMsg")]
        public string ReturnMsg { get; set; }
        [JsonProperty("workId")]
        public string WorkID { get; set; }
        [JsonProperty("deviceId")]
        public string DeviceID { get; set; }

        [JsonProperty("thinqUri")]
        public Uri ThinqUri { get; set; }

        [JsonProperty("empUri")]
        public Uri EmpUri { get; set; }

        [JsonProperty("contentsUri")]
        public Uri ContentsUri { get; set; }

        [JsonProperty("rtiUri")]
        public string RtiUri { get; set; }

        [JsonProperty("cicTel")]
        public string CicTel { get; set; }

        [JsonProperty("oauthUri")]
        public Uri OauthUri { get; set; }

        [JsonProperty("appLatestVer")]
        public string AppLatestVer { get; set; }

        [JsonProperty("appLinkAndroid")]
        public string AppLinkAndroid { get; set; }

        [JsonProperty("appLinkIos")]
        public Uri AppLinkIos { get; set; }

        [JsonProperty("appUpdateYn")]
        public string AppUpdateYn { get; set; }

        [JsonProperty("empOauthErrorYn")]
        public string EmpOauthErrorYn { get; set; }

        [JsonProperty("empOauthDetourUri")]
        public string EmpOauthDetourUri { get; set; }
        [JsonProperty("cmd")]
        public string Cmd { get; set; }
        [JsonProperty("cmdOpt")]
        public string CmdOpt { get; set; }

        [JsonProperty("workList")]
        [JsonConverter(typeof(SingleOrArrayConverter<WorkList>))]
        public WorkList[] WorkList { get; set; }

        [JsonProperty("imageUri")]
        public Uri ImageUri { get; set; }

        [JsonProperty("langPackIntroVer")]
        public double LangPackIntroVer { get; set; }

        [JsonProperty("langPackIntroUri")]
        public Uri LangPackIntroUri { get; set; }

        [JsonProperty("showYn")]
        public string ShowYn { get; set; }

        [JsonProperty("showLocalPushYn")]
        public string ShowLocalPushYn { get; set; }

        [JsonProperty("mediaUri")]
        public string MediaUri { get; set; }

        [JsonProperty("isSupportVideoYn")]
        public string IsSupportVideoYn { get; set; }

        [JsonProperty("langPackCommonVer")]
        public double LangPackCommonVer { get; set; }

        [JsonProperty("langPackCommonUri")]
        public Uri LangPackCommonUri { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("langCode")]
        public string LangCode { get; set; }

        [JsonProperty("countryLangDescription")]
        public string CountryLangDescription { get; set; }

        [JsonProperty("nestSupportAppVer")]
        public string NestSupportAppVer { get; set; }

        [JsonProperty("uuidLoginYn")]
        public string UuidLoginYn { get; set; }

        [JsonProperty("lineLoginYn")]
        public string LineLoginYn { get; set; }
        [JsonProperty("loginType")]
        public string LoginType { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("jsessionId")]
        public string JsessionId { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("clauseReAgree")]
        public string ClauseReAgree { get; set; }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("accountType")]
        public string AccountType { get; set; }

        [JsonProperty("mbrNo")]
        public string MbrNo { get; set; }

        [JsonProperty("noticeSeq")]
        public string NoticeSeq { get; set; }

        [JsonProperty("noticeTitle")]
        public string NoticeTitle { get; set; }

        [JsonProperty("notice")]
        public string Notice { get; set; }

        [JsonProperty("noticeLinkTitle")]
        public string NoticeLinkTitle { get; set; }

        [JsonProperty("noticeLink")]
        public string NoticeLink { get; set; }
        [JsonProperty("item")]
        [JsonConverter(typeof(SingleOrArrayConverter<Device>))]
        public Device[] Item { get; set; }
        [JsonIgnore]
        public Translate CommonTranslate { get; set; }
        [JsonIgnore]
        public Translate InitTranslate { get; set; }
        public LgedmRoot ReplaceWithNewData(LgedmRoot anotherRoot)
        {
            lock (this)
            {
                var properties = GetType().GetProperties();
                foreach (var item in properties)
                {
                    var value = item.GetValue(anotherRoot);
                    if (value != null)
                        item.SetValue(this, value);
                }
            }
            return this;
        }
    }
}
