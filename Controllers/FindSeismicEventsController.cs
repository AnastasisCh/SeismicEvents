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
        public FindSeismicEventsController(SeismicEventsFireEventsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("{flynnRegion}")]
        public async Task<IActionResult> FindSeismicEvents(string flynnRegion)
        {
            IEnumerable<byte[]> compressedSeismicPropertyChunks= _dbContext.SeismicCompressed
                .Where(se => se.FlynnRegion.Equals(flynnRegion))
                .Select(row=>row.CompressedEventProperties).AsEnumerable();
            IEnumerable<DTOs.SeismicProperties> decompressedSeismicProperties = SeismicEventsUtils.DecompressSeismicChunks(compressedSeismicPropertyChunks);
            return Ok(decompressedSeismicProperties);
        }

    }
}
