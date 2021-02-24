using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
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
        [Authorize]
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
                    RequestUri = new Uri($"https://discus.eu.auth0.com/api/v2/users?q={filter}&search_engine=v3&include_totals=true&page=0&per_page=10")
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

        [Authorize]
        [HttpPatch("Me")]
        public async Task<IActionResult> PublishMe([FromBody] Metadata newuser)
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

        [Authorize]
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
        [HttpGet("EventAttendance/{id}")]
        public async Task<IActionResult> UpdateUserProfile(int id)
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
        
    }
}
