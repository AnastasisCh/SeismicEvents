using System.Text.Json.Serialization;

namespace SeismicEventsFireEvents.TestingApproaches.SeedWesternTurkeyData
{
    public class FeaturesDTO
    {
        [JsonPropertyName("features")]
        public TypeDTO[] Features { get; set; }
    }
}
