using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail;

namespace DISCUS_API.Models
{
    public class Email
    {
        [JsonProperty("recipients")]
        public List <EmailAddress> Recipients { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

    }
}
