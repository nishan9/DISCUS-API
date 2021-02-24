using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DISCUS_API.Models
{
    public class EventAttendance
    {

        [JsonProperty("users")]
        public List<EventUser> Users { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

    }

    public class EventUser
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("user_metadata")]
        public ExpertiseInterests User_metadata { get; set; }

    }
    public class ExpertiseInterests
    {
        [JsonProperty("expertise")]
        public List<string> Expertise { get; set; }

        [JsonProperty("interest")]
        public List<string> Interest { get; set; }
    }
}
