using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeismicEventsFireEvents.Data;
using SeismicEventsFireEvents.DTOs;
using SeismicEventsFireEvents.Services;
using SeismicEventsFireEvents.Utils;

namespace SeismicEventsFireEvents.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FindSeismicEventsController : Controller
    {
        private readonly SeismicEventsFireEventsDbContext _dbContext;
        private readonly SeismicEventsDapperDbContext _dapperDbContext;
        public FindSeismicEventsController(SeismicEventsFireEventsDbContext dbContext,SeismicEventsDapperDbContext dapperDbContext)
        {
            _dbContext = dbContext;
            _dapperDbContext = dapperDbContext;
        }
        //~100ms 6.7mb
        [HttpGet("FindSeismicEventsEF/{flynnRegion}")]
        public async Task<IActionResult> FindSeismicEventsEF(string flynnRegion)
        {
            IEnumerable<byte[]> compressedSeismicPropertyChunks= _dbContext.SeismicCompressed
                .Where(se => se.FlynnRegion.Equals(flynnRegion))
                .Select(row=>row.CompressedEventProperties).AsEnumerable();
            IEnumerable<DTOs.SeismicProperties> decompressedSeismicProperties = SeismicEventsUtils.DecompressSeismicChunks(compressedSeismicPropertyChunks);
            return Ok(decompressedSeismicProperties);
        }
        //~70ms 6.7mb
        [HttpGet("FindSeismicEventsDapper/{flynnRegion}")]
        public async Task<IActionResult> FindSeismicEventsDapper(string flynnRegion)
        {
            IEnumerable<byte[]> compressedSeismicPropertiesDapper = Enumerable.Empty<byte[]>();
            using(var connection=_dapperDbContext.CreateConnection())
            {
                compressedSeismicPropertiesDapper = await connection.QueryAsync<byte[]>("SELECT CompressedEventProperties FROM SeismicCompressed WHERE FlynnRegion=@FlynnRegion",new {FlynnRegion=flynnRegion});
            }
            IEnumerable<SeismicProperties> decompressedSeismicPropertiesDapper = SeismicEventsUtils.DecompressSeismicChunks(compressedSeismicPropertiesDapper);
            return Ok(decompressedSeismicPropertiesDapper);
        }
        [HttpGet("Depth")]
        public async Task<IActionResult> FindSeismicEvents([FromQuery] double minDepth, [FromQuery] double maxDepth)
        {
            IEnumerable<byte[]> compressedSeismicPropertyChunks = _dbContext.SeismicCompressed
                .Where(se => se.MinDepth<=maxDepth && se.MaxDepth>=minDepth)
                .Select(row => row.CompressedEventProperties).AsEnumerable();
            IEnumerable<DTOs.SeismicProperties> decompressedSeismicProperties = SeismicEventsUtils.DecompressSeismicChunks(compressedSeismicPropertyChunks).Where(se=>se.Depth>=minDepth && se.Depth<=maxDepth);
            return Ok(decompressedSeismicProperties);
        }

    }
}
