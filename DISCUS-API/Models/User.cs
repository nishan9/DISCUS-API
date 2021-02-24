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

        [JsonProperty("app_metadata")]
        public AppMetadata App_metadata { get; set; }

    }

    public class AppMetadata
    {
        [JsonProperty("isAdmin")]
        public Boolean isAdmin { get; set; }
    }

    public class UserMetadata
    {
        [JsonProperty("social")]
        public Social Social { get; set; }

        [JsonProperty("education")]
        public Education Education { get; set; }

        [JsonProperty("research")]
        public string Research { get; set; }

        [JsonProperty("expertise")]
        public List<string> Expertise { get; set; }

        [JsonProperty("interest")]
        public List<string> Interest { get; set; }

        [JsonProperty("events")]
        public List<int> Events { get; set; }

    }

    public class Metadata
    {
        [JsonProperty("user_metadata")]
        public UserMetadata User_metadata { get; set; }
    }

    public class UpdateUser
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("user_metadata")]
        public UserMetadata User_metadata { get; set; }

    }


    public class Social
    {
        public string sussex { get; set;  }
    }

    public class Education
    {
        public string school { get; set;  }

        public string Department { get; set;  }

        public string CareerStage { get; set;  }

        public string GraduationDate { get; set;  }

        public string Available { get; set;  }

    }

}
