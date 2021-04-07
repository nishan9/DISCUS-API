using System;
using Newtonsoft.Json;

namespace DISCUS_API.Models
{
    public class EmailList
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
