using Microsoft.AspNetCore.Mvc;
using SeismicEventsFireEvents.Data;
using SeismicEventsFireEvents.DTOs;

namespace SeismicEventsFireEvents.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FindSeismicEventRawController : Controller
    {
        private readonly SeismicEventsFireEventsDbContext _dbContext;
        public FindSeismicEventRawController(SeismicEventsFireEventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("{flynnRegion}")]
        public async Task<IActionResult> FindSeismicEvents(string flynnRegion)
        {
            IEnumerable<Models.SeismicProperties> rawEvents= _dbContext.SeismicProperties.Where(row => row.FlynnRegion.Equals(flynnRegion)).AsEnumerable();
            return Ok(rawEvents);
        }
    }
}
