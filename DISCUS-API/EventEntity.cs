using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DISCUS_API
{
    public class EventEntity
    {
            [Key]
            public int Id { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("dateTime")]
            public string DateTime { get; set; }

            [JsonProperty("type")]
            public string Type { get; set;  }

            [JsonProperty("url")]
            public string URL { get; set;  }

            [JsonProperty("description")]
            public string Description { get;  set; }

            [JsonProperty("isDISCUS")]
            public bool IsDISCUS { get; set; }
    }
}
