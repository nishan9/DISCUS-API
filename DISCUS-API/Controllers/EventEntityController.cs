using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DISCUS_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EventEntityController : ControllerBase
    {
        private DataContext context;
        public EventEntityController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {

            List<EventEntity> Entity = await context.EventEntity.ToListAsync();
            return new OkObjectResult(Entity);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            EventEntity eventEntity = context.EventEntity.Where(x => x.Title == name).FirstOrDefault();
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
            return new OkResult();

        }

    }
}
