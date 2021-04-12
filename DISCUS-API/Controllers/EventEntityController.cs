using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using DISCUS_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DISCUS_API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class EventEntityController : ControllerBase
    {
        private HttpClient client;
        private DataContext context;
        private Auth0Settings auth0settings;

        public EventEntityController(HttpClient client, IOptions<Auth0Settings> auth0settings, DataContext context)
        {
            this.client = client;
            this.context = context;
            this.auth0settings = auth0settings.Value;
        }

        [HttpGet("Upcoming")]
        public async Task<IActionResult> GetUpcomingEvents()
        {

            List<EventEntity> Entity = await context.EventEntity.Where(e => e.FinishedDateTime > DateTime.Now && e.IsApproved == true).OrderByDescending(q => q.FinishedDateTime).ToListAsync(); 
            return new OkObjectResult(Entity);
        }

        [HttpGet("Past")]
        public async Task<IActionResult> GetPastEvents()
        {

            List<EventEntity> Entity = await context.EventEntity.Where(e => e.FinishedDateTime < DateTime.Now && e.IsApproved == true).OrderByDescending(q => q.FinishedDateTime).ToListAsync();
            return new OkObjectResult(Entity);
        }

        [HttpGet("Count")]
        public async Task<IActionResult> CountEvents()
        {

            List<EventEntity> Entity = await context.EventEntity.ToListAsync();
            return new OkObjectResult(Entity.Count());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecificEvent(int id)
        {
            EventEntity eventEntity = context.EventEntity.Where(x => x.Id == id).FirstOrDefault();
            if (eventEntity != null)
            {
                return new OkObjectResult(eventEntity);
            }
            else
            {
                return new BadRequestObjectResult("No record found");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEventEntity([FromBody] EventEntity eventEntity)
        {
            await context.EventEntity.AddAsync(eventEntity);
            await context.SaveChangesAsync();
            return new OkObjectResult(eventEntity);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateEventEntity([FromBody] EventEntity neweventEntity, int id)
        {
            neweventEntity.Id = id; 
            EventEntity eventEntity = context.EventEntity.Where(x => x.Id == id).FirstOrDefault();
            if (eventEntity != null)
            {
                eventEntity.Title = neweventEntity.Title;
                eventEntity.Type = neweventEntity.Type;
                eventEntity.URL = neweventEntity.URL;
                eventEntity.IsDISCUS = neweventEntity.IsDISCUS;
                eventEntity.Description = neweventEntity.Description;
                eventEntity.DateTime = neweventEntity.DateTime;
                eventEntity.Tags = neweventEntity.Tags;
                eventEntity.FinishedDateTime = neweventEntity.FinishedDateTime; 

                await context.SaveChangesAsync();
                return new OkObjectResult(eventEntity); 
            }
            else
            {
                return new BadRequestObjectResult("No record found");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventEntity(int id)
        {
            EventEntity eventEntity = new EventEntity() { Id = id };
            context.EventEntity.Attach(eventEntity);
            context.EventEntity.Remove(eventEntity);
            await context.SaveChangesAsync();
            return new OkObjectResult(eventEntity);
        }

        [HttpGet("Unauthorized")]
        public async Task<IActionResult> GetEventToApprove()
        {
            List<EventEntity> Entity = await context.EventEntity.Where(e => e.IsApproved == false).ToListAsync();
            return new OkObjectResult(Entity);
        }

        [Authorize]
        [HttpPatch("Approve/{id}")]
        public async Task<IActionResult> GetEventToApprove(int id)
        {
            string userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            HttpRequestMessage req = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discus.eu.auth0.com/api/v2/users/{userid}/permissions"),
            };
            req.Headers.Add("Authorization", $"Bearer {auth0settings.Auth0ManagmentKey}");
            HttpResponseMessage res = await client.SendAsync(req);
            string jsonString = await res.Content.ReadAsStringAsync();
            Auth0Permissions[] permission = JsonConvert.DeserializeObject<Auth0Permissions[]>(jsonString);
            if (permission[0].Sources[0].Source_name == "Admin")
            {
                EventEntity result = (from p in context.EventEntity where p.Id == id select p).SingleOrDefault();
                result.IsApproved = true;
                await context.SaveChangesAsync();
                return new OkObjectResult("Successful");
            }
            else {
                return new BadRequestObjectResult("Permission Denied");
            }
        }
    }
}
