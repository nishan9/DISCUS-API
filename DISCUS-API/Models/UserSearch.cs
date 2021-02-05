using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DISCUS_API.Models
{
    public class UserSearch
    {

        [JsonProperty("start")]
        public int Start { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("length")]
        public int Length { get; set; }

        [JsonProperty("users")]
        public List<User> Users { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
