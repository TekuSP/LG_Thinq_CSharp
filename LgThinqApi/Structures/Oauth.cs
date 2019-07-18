using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LGThingApi.Structures
{
    [Serializable]
    public class AuthorizationStructure
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }
    }
}
