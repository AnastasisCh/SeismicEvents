using MessagePack;
using SeismicEventsFireEvents.Models;
using System.IO.Compression;

namespace SeismicEventsFireEvents.Utils
{
    public static class SeismicEventsUtils
    {
        public static void UpdateMetadataChunkInfo(SeismicCompressedProperties comporessedInMemoryProperties, DTOs.SeismicProperties dequeuedLiveEvent)
        {
            if (comporessedInMemoryProperties.MinMagnitude is null || comporessedInMemoryProperties.MinMagnitude > dequeuedLiveEvent.Magnitude)
            {
                comporessedInMemoryProperties.MinMagnitude = dequeuedLiveEvent.Magnitude;
            }
            if (comporessedInMemoryProperties.MaxMagnitude is null || comporessedInMemoryProperties.MaxMagnitude < dequeuedLiveEvent.Magnitude)
            {
                comporessedInMemoryProperties.MaxMagnitude = dequeuedLiveEvent.Magnitude;
            }
            if (comporessedInMemoryProperties.FirstEventDate is null || comporessedInMemoryProperties.FirstEventDate > dequeuedLiveEvent.RegistrationDate)
            {
                comporessedInMemoryProperties.FirstEventDate = dequeuedLiveEvent.RegistrationDate;
            }
            if (comporessedInMemoryProperties.LastEventDate is null || comporessedInMemoryProperties.LastEventDate < dequeuedLiveEvent.RegistrationDate)
            {
                comporessedInMemoryProperties.LastEventDate = dequeuedLiveEvent.RegistrationDate;
            }
            if (comporessedInMemoryProperties.MinDepth is null || comporessedInMemoryProperties.MinDepth > dequeuedLiveEvent.Depth)
            {
                comporessedInMemoryProperties.MinDepth = dequeuedLiveEvent.Depth;
            }
            if (comporessedInMemoryProperties.MaxDepth is null || comporessedInMemoryProperties.MaxDepth < dequeuedLiveEvent.Depth)
            {
                comporessedInMemoryProperties.MaxDepth = dequeuedLiveEvent.Depth;
            }
            if (comporessedInMemoryProperties.MinLatitude is null || comporessedInMemoryProperties.MinLatitude > dequeuedLiveEvent.Latitude)
            {
                comporessedInMemoryProperties.MinLatitude = dequeuedLiveEvent.Latitude;
            }
            if (comporessedInMemoryProperties.MaxLatitude is null || comporessedInMemoryProperties.MaxLatitude < dequeuedLiveEvent.Latitude)
            {
                comporessedInMemoryProperties.MaxLatitude = dequeuedLiveEvent.Latitude;
            }
            if (comporessedInMemoryProperties.MinLongitude is null || comporessedInMemoryProperties.MinLongitude > dequeuedLiveEvent.Longitude)
            {
                comporessedInMemoryProperties.MinLongitude = dequeuedLiveEvent.Longitude;
            }
            if (comporessedInMemoryProperties.MaxLongitude is null || comporessedInMemoryProperties.MaxLongitude < dequeuedLiveEvent.Longitude)
            {
                comporessedInMemoryProperties.MaxLongitude = dequeuedLiveEvent.Longitude;
            }
            comporessedInMemoryProperties.EventCount += 1;
        }

        public static byte[] CompressSeismicProperties(IEnumerable<DTOs.SeismicProperties> regionBatchSeismicEvents)
        {
            byte[] bytesOfSeismicEvents = MessagePackSerializer.Serialize(regionBatchSeismicEvents);

            using (MemoryStream output = new MemoryStream())
            {
                using (var gzip = new GZipStream(output, CompressionLevel.Optimal, leaveOpen: true))
                {
                    gzip.Write(bytesOfSeismicEvents, 0, bytesOfSeismicEvents.Length);
                }
                return output.ToArray();
            }
        }
        public static IEnumerable<DTOs.SeismicProperties> DecompressSeismicChunks(IEnumerable<byte[]> compressedPropertyChunks)
        {
            foreach (var compressedChunk in compressedPropertyChunks)
            {
                // foreach chunk , dcompress and deserialize ,gzipSteam starts at byte 0 by default
                // attention thats PER CHUNK , not per event in each chunk
                using var gzip = new GZipStream(new MemoryStream(compressedChunk), CompressionMode.Decompress);
                DTOs.SeismicProperties[] arrayOfSeismicPropertiesPerChunk = MessagePackSerializer.Deserialize<DTOs.SeismicProperties[]>(gzip);
                foreach(var seismicProperty in arrayOfSeismicPropertiesPerChunk)
                {
                    yield return seismicProperty;
                }
               
            }
        }
    }
}
