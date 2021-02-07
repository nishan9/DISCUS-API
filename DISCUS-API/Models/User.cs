using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DISCUS_API.Models
{
    public class User
    {
        [JsonProperty("created_at")]
        public DateTime Created_at { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("email_verified")]
        public bool Email_verified { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("updated_at")]
        public DateTime Updated_at { get; set; }

        [JsonProperty("user_id")]
        public string User_id { get; set; }

        [JsonProperty("user_metadata")]
        public UserMetadata User_metadata { get; set; }

        [JsonProperty("last_login")]
        public DateTime Last_login { get; set; }

        [JsonProperty("last_ip")]
        public string Last_ip { get; set; }

        [JsonProperty("logins_count")]
        public int Logins_count { get; set; }
    }

    public class UserMetadata
    {
        [JsonProperty("career_stage")]
        public string Career_stage { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("linkedin")]
        public string Linkedin { get; set; }

        [JsonProperty("research_interests")]
        public string Research_interests { get; set; }

        [JsonProperty("school")]
        public string School { get; set; }

        [JsonProperty("sussex")]
        public string Sussex { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }
    }

}
