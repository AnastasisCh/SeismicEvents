using MessagePack;
using Microsoft.EntityFrameworkCore;
using SeismicEventsFireEvents.Data;
using SeismicEventsFireEvents.DTOs;
using SeismicEventsFireEvents.Enums;
using SeismicEventsFireEvents.Models;
using System.Collections.Concurrent;
using System.IO.Compression;
using SeismicEventsFireEvents.Utils;

namespace SeismicEventsFireEvents.Services
{
    public class FlushSeismicInMemoryTable : BackgroundService
    {
        public static Queue<DTOs.SeismicProperties> toBeFlushedSeismicEvents = new Queue<DTOs.SeismicProperties>();
        public static ConcurrentDictionary<string, InMemoryFlushQueue> toBeFlushedQueues = new ConcurrentDictionary<string, InMemoryFlushQueue>();
        private readonly IServiceScopeFactory _scopeFactory;

        public FlushSeismicInMemoryTable(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            //foreach (FlinnEngdahlRegion flinnEngdahlRegion in Enum.GetValues<FlinnEngdahlRegion>())
            //{
            //    toBeFlushedQueues.TryAdd(flinnEngdahlRegion.ToString(), new InMemoryFlushQueue
            //    {
            //        PropertiesQueue = new Queue<SeismicProperties>(),
            //        MetadataColumnProperties = new SeismicCompressedProperties
            //        {
            //            FlynnRegion = flinnEngdahlRegion.ToString(),
            //            MinMagnitude = default,
            //            MaxMagnitude = default,
            //            FirstEventDate = default,
            //            LastEventDate = default,
            //            MinDepth = default,
            //            MaxDepth = default,
            //            MinLatitude = default,
            //            MaxLatitude = default,
            //            MinLongitude = default,
            //            MaxLongitude = default,
            //            EventCount = default
            //        }
            //    }
            //    );
            //}
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await DequeueLiveSeismicEvents(SeismicPortalClientWS.SeismicEvents); //cpu bound task
                await Task.Delay(2000, cancellationToken); //wait for 2 seconds before checking again
            }
        }

        private async Task DequeueLiveSeismicEvents(ConcurrentQueue<SeismicPortalMessage> liveSeismicEvents)
        {
            if (liveSeismicEvents.IsEmpty)
            {
                return;
            }
            //How much time should live data stay in memory before being flushed to DB (UI Live Dashboard)
            if (liveSeismicEvents.TryPeek(out SeismicPortalMessage seismicEvent) && DateTime.UtcNow - seismicEvent.Data.Properties.RegistrationDate >= TimeSpan.FromMinutes(1))
            {
                InMemoryFlushQueue? countryQueueOrchestrator = toBeFlushedQueues.Where(EuropeanCountry => seismicEvent.Data.Properties.FlynnRegion.Contains(EuropeanCountry.Key, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault().Value;
                if (countryQueueOrchestrator is null)
                {
                    //log for which region there is no queue based on enum
                    toBeFlushedQueues.TryAdd
                        (seismicEvent.Data.Properties.FlynnRegion, new InMemoryFlushQueue
                            {
                                PropertiesQueue = new Queue<DTOs.SeismicProperties>(),
                                MetadataColumnProperties = new SeismicCompressedProperties
                                {
                                    FlynnRegion = seismicEvent.Data.Properties.FlynnRegion,
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
                                }
                            }
                        );
                    countryQueueOrchestrator = toBeFlushedQueues[seismicEvent.Data.Properties.FlynnRegion];
                }
                Queue<DTOs.SeismicProperties>? countrySeismicEventQueueToBeFushed = countryQueueOrchestrator.PropertiesQueue;
                SeismicCompressedProperties metadataRegionBatchHandler = countryQueueOrchestrator.MetadataColumnProperties;

                if (liveSeismicEvents.TryDequeue(out SeismicPortalMessage dequeuedLiveEvent))
                    countrySeismicEventQueueToBeFushed?.Enqueue(dequeuedLiveEvent.Data.Properties);
                else
                    //possibility of inifinie recursion here if always unable to dequeue
                    await DequeueLiveSeismicEvents(liveSeismicEvents);

                SeismicEventsUtils.UpdateMetadataChunkInfo(metadataRegionBatchHandler, dequeuedLiveEvent.Data.Properties);
                if (countrySeismicEventQueueToBeFushed.Count == 5)
                {
                    byte[] compressedSeismicRecords = SeismicEventsUtils.CompressSeismicProperties(countrySeismicEventQueueToBeFushed);
                    //DBContext needed here so i can insert into blobTable the record with await
                    metadataRegionBatchHandler.CompressedEventProperties = compressedSeismicRecords;
                    metadataRegionBatchHandler.RegistrationDate = DateTime.UtcNow;
                    using (IServiceScope scope = _scopeFactory.CreateScope())
                    {
                        var scopedSeismicFireDb=scope.ServiceProvider.GetRequiredService<SeismicEventsFireEventsDbContext>();
                        await scopedSeismicFireDb.SeismicCompressed.AddAsync(metadataRegionBatchHandler);
                        await scopedSeismicFireDb.SaveChangesAsync();
                    }
                }

                await DequeueLiveSeismicEvents(liveSeismicEvents);
            }
            return;
        }

        

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


    }
}
