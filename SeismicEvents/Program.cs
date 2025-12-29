using Microsoft.EntityFrameworkCore;
using SeismicEventsFireEvents.Data;
using SeismicEventsFireEvents.Services;
using SeismicEventsFireEvents.TestingApproaches.SeedWesternTurkeyData;
using System;

namespace SeismicEventsFireEvents
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.Services.AddHostedService<PollingNASAFirms>();
            // builder.Services.AddHostedService<SeedData>();
            builder.Services.AddHostedService<SeismicPortalClientWS>();
            builder.Services.AddHostedService<FlushSeismicInMemoryTable>();

            builder.Services.AddControllers();
            builder.Services.AddDbContext<SeismicEventsFireEventsDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("SQLLiteConnection")));
            builder.Services.AddScoped<SeismicEventsDapperDbContext>();


            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
