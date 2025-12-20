using Microsoft.EntityFrameworkCore;
using SeismicEventsFireEvents.Models;

namespace SeismicEventsFireEvents.Data
{
    public class SeismicEventsFireEventsDbContext : DbContext
    {
        public SeismicEventsFireEventsDbContext(DbContextOptions<SeismicEventsFireEventsDbContext> options)
           : base(options)
        {
        }
        public DbSet<SeismicCompressedProperties> SeismicCompressed { get; set; }
        public DbSet<SeismicProperties> SeismicProperties { get; set; }
    }
}
