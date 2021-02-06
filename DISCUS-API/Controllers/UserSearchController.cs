using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using DISCUS_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DISCUS_API.Controllers
{

    
    [Route("[controller]")]
    [ApiController]
    public class UserSearchController : Controller
    {

        private HttpClient client;
        private Auth0Settings auth0settings; 

        public UserSearchController(HttpClient client, IOptions<Auth0Settings> auth0settings) {
            this.client = client;
            this.auth0settings = auth0settings.Value; 
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            HttpRequestMessage req = new HttpRequestMessage
            {
                RequestUri = new System.Uri("https://discus.eu.auth0.com/api/v2/users?page=0&per_page=10&include_totals=true")
            };
            req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");  
            HttpResponseMessage res = await client.SendAsync(req);
            string jsonString = await res.Content.ReadAsStringAsync();
            UserSearch result = JsonConvert.DeserializeObject<UserSearch>(jsonString); 
            return new OkObjectResult(result); 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneUser(string id)
        {
            string auth0id = "auth0|" + id;
            HttpRequestMessage req = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discus.eu.auth0.com/api/v2/users/{auth0id}")
            };
            req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");
            HttpResponseMessage res = await client.SendAsync(req);
            string jsonString = await res.Content.ReadAsStringAsync();
            User result = JsonConvert.DeserializeObject<User>(jsonString);
            return new OkObjectResult(result);

        }

        [HttpGet("Search/{name}/{page}")]
        public async Task<IActionResult> GetPageUser(string name, int page)
        {
            HttpRequestMessage req = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discus.eu.auth0.com/api/v2/users?page={page}&per_page=10&include_totals=true&q={name}&search_engine=v3")
            };
            
            req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");
            HttpResponseMessage res = await client.SendAsync(req);
            string jsonString = await res.Content.ReadAsStringAsync();
            UserSearch result = JsonConvert.DeserializeObject<UserSearch>(jsonString);
            return new OkObjectResult(result);

        }
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
                    RequestUri = new Uri($"https://discus.eu.auth0.com/api/v2/users?page={page}&per_page=10&include_totals=true&q={filter}&search_engine=v3")
                };

                req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");
                HttpResponseMessage res = await client.SendAsync(req);
                string jsonString = await res.Content.ReadAsStringAsync();
                UserSearch result = JsonConvert.DeserializeObject<UserSearch>(jsonString);
                return new OkObjectResult(result);

            }
        }

        [Authorize]
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


        [HttpGet("Tags")]
        public async Task<IActionResult> GetTags()
        {
            TagSystem tagSystem = new TagSystem 
            {
                Subject = new List<SubjectType>
                {
                    new SubjectType
                    {
                        Subject = "Biology",
                        Children = new List<SubjectType>
                        {
                            new SubjectType
                            {
                                Subject= "Geo",
                                Children= "null"
                            },
                        }
                    }
                }
            };

            tagSystem.Subject.Add(new SubjectType() { Subject = "Physics", Children = "None" });
            tagSystem.Subject.Add(new SubjectType() { Subject = "Mathematics", Children = "None" });

            return new OkObjectResult(tagSystem);
        }

        [HttpPost("AddSubject")]
        public async Task<IActionResult> AddSubject([FromBody] SubjectType newsubject)
        {
            TagSystem tagSystem = new TagSystem
            {
                Subject = new List<SubjectType>
                {
                    new SubjectType
                    {
                        Subject = "Biology",
                        Children = new List<SubjectType>
                        {
                            new SubjectType
                            {
                                Subject= "Geo",
                                Children= "null"
                            },
                        }
                    }
                }
            };
            tagSystem.Subject.Add(newsubject); 

            return new OkObjectResult(tagSystem);
        }

        [HttpPost("AddSubject/{param}")]
        public async Task<IActionResult> AddTo([FromBody] SubjectType newsubject, string subject)
        {
            TagSystem tagSystem = new TagSystem
            {
                Subject = new List<SubjectType>
                {
                    new SubjectType
                    {
                        Subject = "Biology",
                        Children = new List<SubjectType>
                        {

                            new SubjectType
                            {
                                Subject = newsubject.Subject,
                                Children = newsubject.Children
                            }
                        }
                    },
                }
            };

            return new OkObjectResult(tagSystem);
        }

    }
}
