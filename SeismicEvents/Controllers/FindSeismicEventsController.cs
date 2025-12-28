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
        [HttpGet("Compressed/FindSeismicEventsEF/{flynnRegion}")]
        public async Task<IActionResult> FindSeismicEventsEF(string flynnRegion)
        {
            IEnumerable<byte[]> compressedSeismicPropertyChunks= _dbContext.SeismicCompressed
                .Where(se => se.FlynnRegion.Equals(flynnRegion))
                .Select(row=>row.CompressedEventProperties).AsEnumerable();
            IEnumerable<DTOs.SeismicProperties> decompressedSeismicProperties = SeismicEventsUtils.DecompressSeismicChunks(compressedSeismicPropertyChunks);
            return Ok(decompressedSeismicProperties);
        }
        //~100ms 6.7mb
        [HttpGet("Compressed/FindSeismicEventsDapper/{flynnRegion}")]
        public async Task<IActionResult> FindSeismicEventsDapper(string flynnRegion)
        {
            IEnumerable<byte[]> compressedSeismicPropertiesDapper = Enumerable.Empty<byte[]>();
            using(var connection=_dapperDbContext.CreateConnection())
            {
                compressedSeismicPropertiesDapper = await connection.QueryAsync<byte[]>(@"SELECT CompressedEventProperties 
                                                                                            FROM SeismicCompressed 
                                                                                            WHERE FlynnRegion=@FlynnRegion",new {FlynnRegion=flynnRegion});
            }
            IEnumerable<SeismicProperties> decompressedSeismicPropertiesDapper = SeismicEventsUtils.DecompressSeismicChunks(compressedSeismicPropertiesDapper);
            return Ok(decompressedSeismicPropertiesDapper);
        }
        [HttpGet("Compressed/FindSeismicDepthEF")]
        public async Task<IActionResult> FindSeismicEventsEF([FromQuery] double minDepth = 0, [FromQuery] double maxDepth= 0)
        {
            IEnumerable<byte[]> compressedSeismicPropertyChunks = _dbContext.SeismicCompressed
                .Where(se => se.MinDepth<=maxDepth && se.MaxDepth>=minDepth)
                .Select(row => row.CompressedEventProperties).AsEnumerable();
            IEnumerable<DTOs.SeismicProperties> decompressedSeismicProperties = SeismicEventsUtils.DecompressSeismicChunks(compressedSeismicPropertyChunks).Where(se=>se.Depth>=minDepth && se.Depth<=maxDepth);
            return Ok(decompressedSeismicProperties);
        }
        [HttpGet("Compressed/FindSeismicDepthDapper")]
        public async Task<IActionResult> FindSeismicEventsDapper([FromQuery] double minDepth=0, [FromQuery] double maxDepth=0)
        {
            IEnumerable<byte[]> comporessedProperties = Enumerable.Empty<byte[]>();
            using (var connection=_dapperDbContext.CreateConnection())
            {
                comporessedProperties = await connection.QueryAsync<byte[]>(@"SELECT CompressedEventProperties 
                                                                            FROM SeismicCompressed 
                                                                            WHERE MinimumDepth<=@MaxDepth AND MaximumDepth>=@MinDepth", new { MaxDepth = maxDepth, MinDepth = minDepth });
                
            }
            IEnumerable<SeismicProperties> decompressedSeismicPropertiesDapper = SeismicEventsUtils.DecompressSeismicChunks(comporessedProperties).Where(se => se.Depth >= minDepth && se.Depth <= maxDepth);
            return Ok(decompressedSeismicPropertiesDapper);
        }

    }
}
