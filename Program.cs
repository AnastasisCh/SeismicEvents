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

            // Add services to the container.

            //builder.Services.AddHostedService<PollingNASAFirms>();
            //builder.Services.AddHostedService<SeedData>();
            builder.Services.AddHostedService<SeismicPortalClientWS>();
            builder.Services.AddHostedService<FlushSeismicInMemoryTable>();

            builder.Services.AddControllers();
            builder.Services.AddDbContext<SeismicEventsFireEventsDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
