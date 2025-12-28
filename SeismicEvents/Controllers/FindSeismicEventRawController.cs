using Dapper;
using Microsoft.AspNetCore.Mvc;
using SeismicEventsFireEvents.Data;
using SeismicEventsFireEvents.DTOs;
using SeismicEventsFireEvents.Utils;

namespace SeismicEventsFireEvents.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FindSeismicEventRawController : Controller
    {
        private readonly SeismicEventsFireEventsDbContext _dbContext;
        private readonly SeismicEventsDapperDbContext _dapperDbContext;
        public FindSeismicEventRawController(SeismicEventsFireEventsDbContext dbContext , SeismicEventsDapperDbContext dapperDbContext)
        {
            _dbContext = dbContext;
            _dapperDbContext = dapperDbContext;
        }
        //~300ms 6.7mb
        [HttpGet("Raw/FindSeismicEventsEF/{flynnRegion}")]
        public async Task<IActionResult> FindSeismicEventsEF(string flynnRegion)
        {
            IEnumerable<Models.SeismicProperties> rawEvents= _dbContext.SeismicProperties.Where(row => row.FlynnRegion.Equals(flynnRegion)).AsEnumerable();
            return Ok(rawEvents);
        }
        //~200ms 6.7mb
        [HttpGet("Raw/FindSeismicEventsDapper/{flynnRegion}")]
        public async Task<IActionResult> FindSeismicEventsDapper(string flynnRegion)
        {
            IEnumerable<DTOs.SeismicProperties> seismicProperties=Enumerable.Empty<DTOs.SeismicProperties>();
            using (var connection = _dapperDbContext.CreateConnection())
            {
                seismicProperties=await connection.QueryAsync<DTOs.SeismicProperties>(@"SELECT *
                                                FROM SeismicProperties
                                                WHERE FlynnRegion = @FlynnRegion", new { FlynnRegion = flynnRegion });
            } 
            
            return Ok(seismicProperties);
        }
        [HttpGet("Raw/FindSeismicDepthEF")]
        public async Task<IActionResult> FindSeismicEventsEF([FromQuery]double minDepth=0,[FromQuery] double maxDepth=0)
        {
            IEnumerable<Models.SeismicProperties> rawEvents = _dbContext.SeismicProperties
                .Where(se => se.Depth >= minDepth && se.Depth <= maxDepth).AsEnumerable();
            return Ok(rawEvents);
        }
        [HttpGet("Raw/FindSeismicDepthDapper")]
        public async Task<IActionResult> FindSeismicEventsDapper([FromQuery] double minDepth=0, [FromQuery] double maxDepth=0)
        {
            IEnumerable<DTOs.SeismicProperties> seismicProperties = Enumerable.Empty<DTOs.SeismicProperties>();
            using (var dapperConnection = _dapperDbContext.CreateConnection())
            {
                seismicProperties=await dapperConnection.QueryAsync<DTOs.SeismicProperties>(@"SELECT *
                                                FROM SeismicProperties
                                                WHERE Depth >= @MinDepth AND Depth <= @MaxDepth", new { MinDepth = minDepth, MaxDepth = maxDepth });
            }
            return Ok(seismicProperties);
        }
    }
}
