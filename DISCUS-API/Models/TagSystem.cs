using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DISCUS_API.Models
{
    public class TagSystem
    {
        [JsonProperty("user_metadata")]
        public ExpertiseList User_metadata { get; set; }
    }
    public class ExpertiseList
    {
        [JsonProperty("expertise")]
        public List<string> Expertise { get; set; }
    }

    public class TagSystemInterest
    {
        [JsonProperty("user_metadata")]
        public InterestList User_metadata { get; set; }
    }
    public class InterestList
    {
        [JsonProperty("interest")]
        public List<string> Interest { get; set; }
    }

    public class NivoModel
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }
    }
}
