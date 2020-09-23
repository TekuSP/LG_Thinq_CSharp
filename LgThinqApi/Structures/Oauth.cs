using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LGThingApi.Structures
{
    public class AuthorizationStructure
    {
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
