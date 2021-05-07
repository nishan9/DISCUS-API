using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DISCUS_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DISCUS_API.Controllers
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserSearchController : Controller
    {

        private HttpClient client;
        private Auth0Settings auth0settings;
        private SendGridSettings sendGridSettings; 

        public UserSearchController(HttpClient client, IOptions<Auth0Settings> auth0settings, IOptions<SendGridSettings> sendGridSettings)
        {
            this.client = client;
            this.auth0settings = auth0settings.Value;
            this.sendGridSettings = sendGridSettings.Value; 
        }

        /// <summary>
        /// Retrieves all users from Auth0
        /// </summary>
        /// <returns>UserSearch obejct with Users</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            HttpRequestMessage req = new HttpRequestMessage
            {
                RequestUri = new Uri("https://discus.eu.auth0.com/api/v2/users?page=0&per_page=10&include_totals=true")
            };
            req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");  
            HttpResponseMessage res = await client.SendAsync(req);
            string jsonString = await res.Content.ReadAsStringAsync();
            UserSearch result = JsonConvert.DeserializeObject<UserSearch>(jsonString); 
            return new OkObjectResult(result); 
        }

        /// <summary>
        /// Retrieves details of one specific user
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>Retrives the User object will all relevant information</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneUser(string id)
        {
            HttpRequestMessage req = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discus.eu.auth0.com/api/v2/users/{id}")
            };
            req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");
            HttpResponseMessage res = await client.SendAsync(req);
            string jsonString = await res.Content.ReadAsStringAsync();
            User result = JsonConvert.DeserializeObject<User>(jsonString);
            return new OkObjectResult(result);

        }

        /// <summary>
        /// Updates the User with the information in the body in Auth0. 
        /// </summary>
        /// <param name="newuser">User object is JSON</param>
        /// <returns>The returned user object after manipulation</returns>
        [HttpPatch]
        public async Task<IActionResult> UpdateUser([FromBody] User newuser)
        {
            string jwt = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            HttpRequestMessage req = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discus.eu.auth0.com/api/v2/users/{jwt}"),
                Content = new StringContent(JsonConvert.SerializeObject(newuser), Encoding.UTF8, "application/json")
        };
            req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");
            HttpResponseMessage res = await client.SendAsync(req);
            string jsonString = await res.Content.ReadAsStringAsync();
            User result = JsonConvert.DeserializeObject<User>(jsonString);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// Deletes a specific user with the id in Auth0.  
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>Returns the deleted user</returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            HttpRequestMessage req = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discus.eu.auth0.com/api/v2/users/{id}"),
                Method = HttpMethod.Delete
            };
            req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");
            HttpResponseMessage res = await client.SendAsync(req);
            string jsonString = await res.Content.ReadAsStringAsync();
            User result = JsonConvert.DeserializeObject<User>(jsonString);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// Retrieves search result for the specific page given the search parameter as JSON with up to 50 each time.
        /// </summary>
        /// <param name="name">Query Parameter</param>
        /// <param name="page">Page Number</param>
        /// <returns></returns>
        [HttpGet("Search/{name}/{page}")]
        public async Task<IActionResult> GetPageUser(string query, int page)
        {
            HttpRequestMessage req = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discus.eu.auth0.com/api/v2/users?page={page}&per_page=10&include_totals=true&q={query}&search_engine=v3")
            };
            
            req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");
            HttpResponseMessage res = await client.SendAsync(req);
            string jsonString = await res.Content.ReadAsStringAsync();
            UserSearch result = JsonConvert.DeserializeObject<UserSearch>(jsonString);
            return new OkObjectResult(result);

        }

        /// <summary>
        /// Retrieves search result for the specific page given the search parameter as JSON with up to 50 each time and if specified ALL will resort to no query applied.
        /// </summary>
        /// <param name="page">Page Number</param>
        /// <param name="filter">Search Filter</param>
        /// <returns></returns>
        [HttpGet("Page/{page}/{filter}")]
        public async Task<IActionResult> GetPage(int page, string filter)
        {
            if (filter == "ALL")
            {
                HttpRequestMessage req = new HttpRequestMessage
                {
                    RequestUri = new Uri($"https://discus.eu.auth0.com/api/v2/users?page={page}&per_page=10&include_totals=true&")
                };

                req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");
                HttpResponseMessage res = await client.SendAsync(req);
                string jsonString = await res.Content.ReadAsStringAsync();
                UserSearch result = JsonConvert.DeserializeObject<UserSearch>(jsonString);
                return new OkObjectResult(result);
            }
            else
            {
                HttpRequestMessage req = new HttpRequestMessage
                {
                    RequestUri = new Uri($"https://discus.eu.auth0.com/api/v2/users?q={filter}&search_engine=v3&include_totals=true&page=0&per_page=10")
                };

                req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");
                HttpResponseMessage res = await client.SendAsync(req);
                string jsonString = await res.Content.ReadAsStringAsync();
                UserSearch result = JsonConvert.DeserializeObject<UserSearch>(jsonString);
                return new OkObjectResult(result);

            }
        }

        /// <summary>
        /// Retrieves user information specified in the JWT Token from Auth0. 
        /// </summary>
        /// <returns>The user object</returns>
        [HttpGet("Me")]
        public async Task<IActionResult> GetMe()
        {
            string jwt = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            HttpRequestMessage req = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discus.eu.auth0.com/api/v2/users/{jwt}")
            };
            req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");
            HttpResponseMessage res = await client.SendAsync(req);
            string jsonString = await res.Content.ReadAsStringAsync();
            User result = JsonConvert.DeserializeObject<User>(jsonString);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// Update the user of the Authorization token
        /// </summary>
        /// <param name="newuser"> Metadata that will be replaced with</param>
        /// <returns></returns>
        [HttpPatch("Me")]
        public async Task<IActionResult> UpdateMe([FromBody] Metadata newuser)
        {
            string jwt = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            HttpRequestMessage req = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discus.eu.auth0.com/api/v2/users/{jwt}"),
                Content = new StringContent(JsonConvert.SerializeObject(newuser), Encoding.UTF8, "application/json"),
                Method = HttpMethod.Patch
            };
            req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");
            HttpResponseMessage res = await client.SendAsync(req);
            string jsonString = await res.Content.ReadAsStringAsync();
            User result = JsonConvert.DeserializeObject<User>(jsonString);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// Updates the supplied user in Auth0 
        /// </summary>
        /// <param name="newuser">The deatils to be updated in JSON</param>
        /// <param name="id">Auth0 User ID</param>
        /// <returns></returns>
        [HttpPatch("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUser newuser, string id)
        {
            HttpRequestMessage req = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discus.eu.auth0.com/api/v2/users/{id}"),
                Content = new StringContent(JsonConvert.SerializeObject(newuser), Encoding.UTF8, "application/json"),
                Method = HttpMethod.Patch
            };
            req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");
            HttpResponseMessage res = await client.SendAsync(req);
            string jsonString = await res.Content.ReadAsStringAsync();
            User result = JsonConvert.DeserializeObject<User>(jsonString);
            return new OkObjectResult(result);
        }
        /// <summary>
        /// Restructures Auth0 user's expertise into a model for be graphed
        /// </summary>
        /// <returns>JSON data for a piechart</returns>
        [HttpGet("PieChart/Expertise")]
        public async Task<IActionResult> GetExpertTags()
        {
            HttpRequestMessage req = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discus.eu.auth0.com/api/v2/users?include_fields=true&fields=user_metadata.interest,user_metadata.expertise"),
            };
            req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");
            HttpResponseMessage res = await client.SendAsync(req);
            string jsonString = await res.Content.ReadAsStringAsync();
            List<TagSystem> result = JsonConvert.DeserializeObject <List<TagSystem>>(jsonString);

            List<string> expl = new List<string>(); 
            foreach (TagSystem item in result)
            {
                if (item.User_metadata != null)
                {
                    expl.AddRange(item.User_metadata.Expertise);
                }
            }

            List< NivoModel> response= new List<NivoModel>();

            foreach (string subject in expl)
            {
                int number = expl.Where(x => x.Equals(subject)).Count();
                if (!response.Any(x => x.ID == subject))
                {
                    response.Add(new NivoModel { ID = subject, Value = number });
                }
            }

            var top10 = response.OrderByDescending(o => o.Value).Take(10);
            return new OkObjectResult(top10);
        }
        /// <summary>
        /// Restructures the data from Auth0 to a model to graph as a pie chart
        /// </summary>
        /// <returns>A list of interest tags with their corresponding count</returns>
        [HttpGet("PieChart/Interest")]
        public async Task<IActionResult> GetInterestTags()
        {
            HttpRequestMessage req = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discus.eu.auth0.com/api/v2/users?include_fields=true&fields=user_metadata.interest"),
            };
            req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");
            HttpResponseMessage res = await client.SendAsync(req);
            string jsonString = await res.Content.ReadAsStringAsync();
            List<TagSystemInterest> result = JsonConvert.DeserializeObject<List<TagSystemInterest>>(jsonString);


            List<string> expl = new List<string>();
            foreach (TagSystemInterest item in result)
            {
                if (item.User_metadata != null)
                {
                    expl.AddRange(item.User_metadata.Interest);
                }
            }


            List<NivoModel> response = new List<NivoModel>();

            foreach (string subject in expl)
            {
                int number = expl.Where(x => x.Equals(subject)).Count();
                if (!response.Any(x => x.ID == subject))
                {
                    response.Add(new NivoModel { ID = subject, Value = number });
                }
            }

            var top10 = response.OrderByDescending(o => o.Value).Take(10);

            return new OkObjectResult(top10);
        }
        /// <summary>
        /// Retrieves the numbers of users with the supplied event ID
        /// </summary>
        /// <param name="id">Event ID</param>
        /// <returns>Users attending the specific event</returns>
        [HttpGet("EventAttendance/{id}")]
        public async Task<IActionResult> GetAttendance(int id)
        {
            HttpRequestMessage req = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discus.eu.auth0.com/api/v2/users?q=user_metadata.events:{id}&include_totals=true&fields=total,name,picture,email,user_metadata.expertise,user_metadata.interest&include_fields=true"),
            };
            req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");
            HttpResponseMessage res = await client.SendAsync(req);
            string jsonString = await res.Content.ReadAsStringAsync();
            EventAttendance result = JsonConvert.DeserializeObject<EventAttendance>(jsonString);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// Retrieves number of active users from Auth0. 
        /// </summary>
        /// <returns>Retrieves the number</returns>
        [HttpGet("ActiveUsers")]
        public async Task<IActionResult> GetActiveUsers()
        {
            HttpRequestMessage req = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discus.eu.auth0.com/api/v2/stats/active-users"),
            };
            req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");
            HttpResponseMessage res = await client.SendAsync(req);
            string jsonString = await res.Content.ReadAsStringAsync();
            int result = JsonConvert.DeserializeObject<int>(jsonString);
            return new OkObjectResult(result);
        }
        /// <summary>
        /// Retrieves the total number of users. 
        /// </summary>
        /// <returns>Returns the number of users</returns>
        [HttpGet("TotalUsers")]
        public async Task<IActionResult> GetTotalUsers()
        {
            HttpRequestMessage req = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discus.eu.auth0.com/api/v2/users?include_totals=true&include_fields=true&fields=name"),
            };
            req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");
            HttpResponseMessage res = await client.SendAsync(req);
            string jsonString = await res.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject<dynamic>(jsonString);
            int total = result.total; 
            return new OkObjectResult(total);
        }
        /// <summary>
        /// Retrives a list of emails of all the users from Auth0
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEmails")]
        public async Task<IActionResult> GetEmailsAll()
        {
            HttpRequestMessage req = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discus.eu.auth0.com/api/v2/users?include_fields=true&fields=email,name"),
            };
            req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");
            HttpResponseMessage res = await client.SendAsync(req);
            string jsonString = await res.Content.ReadAsStringAsync();

            List<EmailList> result = JsonConvert.DeserializeObject<List<EmailList>>(jsonString);

            return new OkObjectResult(result);
        }

        /// <summary>
        /// Sends an email with SendGrid client. 
        /// </summary>
        /// <param name="newemail"></param>
        /// <returns></returns>
        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] Email newemail)
        {
            var client = new SendGridClient(sendGridSettings.SENDGRID_API_KEY);
            var from = new EmailAddress("admin@discuslinks.co.uk", "DISCUS");
            var subject = newemail.Subject; 
            var plainTextContent = "";
            var htmlContent = newemail.Body;
            List<EmailAddress> tos = newemail.Recipients; 
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return new OkObjectResult(response);
        }
        /// <summary>
        /// Retrieves data of users' schools and departments which is restructured into a Nivo model of a stacked bar chart. 
        /// </summary>
        /// <returns>The number of users per school per department</returns>
        [HttpGet("Chart")]
        public async Task<IActionResult> SchoolDepList()
        {
            HttpRequestMessage req = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discus.eu.auth0.com/api/v2/users?include_fields=true&fields=user_metadata.education.Department,user_metadata.education.school"),
            };
            req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");
            HttpResponseMessage res = await client.SendAsync(req);
            string jsonString = await res.Content.ReadAsStringAsync();
            List<SchoolDepartment> result = JsonConvert.DeserializeObject<List<SchoolDepartment>>(jsonString);

            List<ScholDep> response = new List<ScholDep>();  
            foreach (SchoolDepartment item in result)
            {
                if (item.User_metadata != null)
                {
                    response.Add(new ScholDep { School = item.User_metadata.Education.Department, Department = item.User_metadata.Education.Department });
                }
            };

            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (ScholDep item in response)
            {
                if (dict.ContainsKey(item.Department))
                {
                    dict[item.Department]++; 
                }
                else
                {
                    dict[item.Department] = 1;
                }
            }

            List<dynamic> AllSchools = new List<dynamic>();

            AllSchools.Add(new
            {
                name = "Business",
                caption = new string[] { "Accounting and Finance", "Economics", "Management", "Strategy and Marketing", "Science Policy Research Unit"},
                Bar1 = dict.ContainsKey("Accounting and Finance") ? dict["Accounting and Finance"] : 0, 
                Bar2 = dict.ContainsKey("Economics") ? dict["Economics"] : 0,
                Bar3 = dict.ContainsKey("Management") ? dict["Management"] : 0,
                Bar4 = dict.ContainsKey("Strategy and Marketing") ? dict["Strategy and Marketing"] : 0,
                Bar5 = dict.ContainsKey("Science Policy Research Unit") ? dict["Science Policy Research Unit"] : 0,
            });

            AllSchools.Add(new
            {
                name = "Edu / Social Work",
                caption = new string[] { "Education", "Social Work and Social Care" },
                Bar1 = dict.ContainsKey("Education") ? dict["Education"] : 0,
                Bar2 = dict.ContainsKey("Social Work and Social Care") ? dict["Social Work and Social Care"] : 0,
            });

            AllSchools.Add(new
            {
                name = "Eng / Inf",
                caption = new string[] { "Engineering", "Informatics", "Product Design" },
                Bar1 = dict.ContainsKey("Engineering") ? dict["Engineering"] : 0,
                Bar2 = dict.ContainsKey("Informatics") ? dict["Informatics"] : 0,
                Bar3 = dict.ContainsKey("Product Design") ? dict["Product Design"] : 0,
            });

            AllSchools.Add(new
            {
                name = "Global",
                caption = new string[] { "Anthropology", "Geography", "International Development", "International Relations" },
                Bar1 = dict.ContainsKey("Anthropology") ? dict["Anthropology"] : 0,
                Bar2 = dict.ContainsKey("Geography") ? dict["Geography"] : 0,
                Bar3 = dict.ContainsKey("International Development") ? dict["International Development"] : 0,
                Bar4 = dict.ContainsKey("International Relations") ? dict["International Relations"] : 0,

            });


            AllSchools.Add(new
            {
                name = "LPS",
                caption = new string[] { "Law", "Politics", "Sociology"},
                Bar1 = dict.ContainsKey("Law") ? dict["Law"] : 0,
                Bar2 = dict.ContainsKey("Politics") ? dict["Politics"] : 0,
                Bar3 = dict.ContainsKey("Sociology") ? dict["Sociology"] : 0,

            });

            AllSchools.Add(new
            {
                name = "Life Sciences",
                caption = new string[] { "Biochemistry and Biomedicine", "Chemistry", "Evolution, behaviour and environment", "Genome damage and stability", "Neuroscience", "Pharmacy" },
                Bar1 = dict.ContainsKey("Biochemistry and Biomedicine") ? dict["Biochemistry and Biomedicine"] : 0,
                Bar2 = dict.ContainsKey("Chemistry") ? dict["Chemistry"] : 0,
                Bar3 = dict.ContainsKey("Evolution, behaviour and environment") ? dict["Evolution, behaviour and environment"] : 0,
                Bar4 = dict.ContainsKey("Genome damage and stability") ? dict["Genome damage and stability"] : 0,
                Bar5 = dict.ContainsKey("Neuroscience") ? dict["Neuroscience"] : 0,
                Bar6 = dict.ContainsKey("Pharmacy") ? dict["Pharmacy"] : 0,

            });

            AllSchools.Add(new
            {
                name = "Maths / Physics",
                caption = new string[] { "Mathematics", "Physics and Astronomy" },
                Bar1 = dict.ContainsKey("Mathematics") ? dict["Mathematics"] : 0,
                Bar2 = dict.ContainsKey("Physics and Astronomy") ? dict["Physics and Astronomy"] : 0,
            });

            AllSchools.Add(new
            {
                name = "Humanities",
                caption = new string[] { "Art History", "Cultural Studies", "Drama", "English literature", "Film Studies", "History", "Journalism", "Language and linguistics", "Media and Communications", "Media Practice", "Music", "Philosophy", "Sussex Centre for American Studies", "Sussex Centre for Language Studies"},
                bar1 = dict.ContainsKey("Art History") ? dict["Art History"] : 0,
                bar2 = dict.ContainsKey("Cultural Studies") ? dict["Cultural Studies"] : 0,
                bar3 = dict.ContainsKey("Drama") ? dict["Drama"] : 0,
                bar5 = dict.ContainsKey("English literature") ? dict["English literature"] : 0,
                bar6 = dict.ContainsKey("Film Studies") ? dict["Film Studies"] : 0,
                bar7 = dict.ContainsKey("History") ? dict["History"] : 0,
                bar8 = dict.ContainsKey("Journalism") ? dict["History"] : 0,
                bar9 = dict.ContainsKey("Language and linguistics") ? dict["Language and linguistics"] : 0,
                bar10 = dict.ContainsKey("Media and Communications") ? dict["Media and Communications"] : 0,
                bar11 = dict.ContainsKey("Media Practice") ? dict["Media Practice"] : 0,
                bar12 = dict.ContainsKey("Music") ? dict["Music"] : 0,
                bar13 = dict.ContainsKey("Philosophy") ? dict["Philosophy"] : 0,
                bar14 = dict.ContainsKey("Sussex Centre for American Studies") ? dict["Sussex Centre for American Studies"] : 0,
                bar15 = dict.ContainsKey("Sussex Centre for Language Studies") ? dict["Sussex Centre for Language Studies"] : 0
            });

            AllSchools.Add(new
            {
                name = "Psych",
                caption = new string[] { "School of Psychology" },
                Bar1 = dict.ContainsKey("School of Psychology") ? dict["School of Psychology"] : 0,
            });

            AllSchools.Add(new
            {
                name = "BSMS",
                caption = new string[] { "Brighton and Sussex Medical School" },
                Bar1 = dict.ContainsKey("Brighton and Sussex Medical School") ? dict["Brighton and Sussex Medical School"] : 0,
            });

            AllSchools.Add(new
            {
                name = "DSRG",
                caption = new string[] { "Doctoral School", "Research groups" },
                Bar1 = dict.ContainsKey("Doctoral School") ? dict["Doctoral School"] : 0,
                Bar2 = dict.ContainsKey("Research groups") ? dict["Research groups"] : 0,
            });

            return new OkObjectResult(AllSchools);
        }
    }
}
