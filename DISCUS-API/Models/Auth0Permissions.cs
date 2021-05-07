using System;
using Newtonsoft.Json;

namespace DISCUS_API.Models
{
    public class Auth0Permissions
    {
        [JsonProperty("sources")]
        public Sources[] Sources { get; set; }
    }

    public class Sources
    {
        [JsonProperty("source_name")]
        public string Source_name { get; set; }

        [JsonProperty("source_id")]
        public string Source_id { get; set; }
    }
}
