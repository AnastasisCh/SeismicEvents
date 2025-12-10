using Newtonsoft.Json;
using SeismicEventsFireEvents.DTOs;
using SeismicEventsFireEvents.Data;
using SeismicEventsFireEvents.Models;

namespace SeismicEventsFireEvents.TestingApproaches.SeedWesternTurkeyData
{
    public class SeedData:BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public SeedData(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;     
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "TestingApproaches\\SeedWesternTurkeyData\\WesternTurkeyData.json");
            FeaturesDTO? features=JsonConvert.DeserializeObject<FeaturesDTO>(File.ReadAllText(path));
            
            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                SeismicEventsFireEventsDbContext context = scope.ServiceProvider.GetRequiredService<SeismicEventsFireEventsDbContext>();
                await context.SeismicProperties.AddRangeAsync(features.Features.Where(f => f.Properties.FlynnRegion.Contains("WESTERN TURKEY", StringComparison.OrdinalIgnoreCase))
                .Select(f => new Models.SeismicProperties
                {
                    SourceId = f.Properties.SourceId,
                    Latitude = f.Properties.Latitude,
                    Longitude = f.Properties.Longitude,
                    Depth = f.Properties.Depth,
                    Magnitude = f.Properties.Magnitude,
                    MagnitudeType = f.Properties.MagnitudeType,
                    SourceCatalog = f.Properties.SourceCatalog,
                    LastUpdate = f.Properties.LastUpdate,
                    EventType = f.Properties.EventType,
                    Author = f.Properties.Author,
                    Unid = f.Properties.Unid,
                    RegistrationDate = DateTime.UtcNow,
                    Time = f.Properties.Time,
                    FlynnRegion = f.Properties.FlynnRegion
                }
                ));
                await context.SaveChangesAsync();
            }
                

            #region AddingChunks

            int acc = 0;
            SeismicCompressedProperties seismicCompressedProperties = new SeismicCompressedProperties
            {
                FlynnRegion = "WESTERN TURKEY",
                MinMagnitude = default,
                MaxMagnitude = default,
                FirstEventDate = default,
                LastEventDate = default,
                MinDepth = default,
                MaxDepth = default,
                MinLatitude = default,
                MaxLatitude = default,
                MinLongitude = default,
                MaxLongitude = default,
                EventCount = default
            };
            IEnumerable<DTOs.SeismicProperties> propertiesToBeChunked = features.Features.Where(f => f.Properties.FlynnRegion.Contains("WESTERN TURKEY", StringComparison.OrdinalIgnoreCase)).Select(f=>f.Properties).ToList();
            foreach (DTOs.SeismicProperties seismicProperties in propertiesToBeChunked)
            {
                if((acc % 1000 == 0 && acc!=0)|| (acc+1)==propertiesToBeChunked.Count())
                {
                    if((acc + 1) == propertiesToBeChunked.Count())
                        acc += 1;
                    Utils.SeismicEventsUtils.UpdateMetadataChunkInfo(seismicCompressedProperties, seismicProperties);
                    byte[] compressedData=Utils.SeismicEventsUtils.CompressSeismicProperties(propertiesToBeChunked.Skip((((acc)/1000)-1)*1000).Take(1000));
                    seismicCompressedProperties.CompressedEventProperties = compressedData;
                    using (IServiceScope scope = _scopeFactory.CreateScope())
                    {
                        SeismicEventsFireEventsDbContext context = scope.ServiceProvider.GetRequiredService<SeismicEventsFireEventsDbContext>();
                        await context.AddAsync(seismicCompressedProperties);
                        await context.SaveChangesAsync();
                    }
                    seismicCompressedProperties = new SeismicCompressedProperties
                    {
                        FlynnRegion = "WESTERN TURKEY",
                        MinMagnitude = default,
                        MaxMagnitude = default,
                        FirstEventDate = default,
                        LastEventDate = default,
                        MinDepth = default,
                        MaxDepth = default,
                        MinLatitude = default,
                        MaxLatitude = default,
                        MinLongitude = default,
                        MaxLongitude = default,
                        EventCount = default
                    };
                    acc += 1;
                    continue;
                }
                Utils.SeismicEventsUtils.UpdateMetadataChunkInfo(seismicCompressedProperties, seismicProperties);
                acc += 1;
            }

            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                SeismicEventsFireEventsDbContext context = scope.ServiceProvider.GetRequiredService<SeismicEventsFireEventsDbContext>();
                await context.SaveChangesAsync();
            }
            #endregion
        }
    }
}
