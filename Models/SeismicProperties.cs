using Newtonsoft.Json;

namespace SeismicEventsFireEvents.Models
{
    public class SeismicProperties
    {
        public int Id { get; set; }
        public string SourceId { get; set; }

        
        public string SourceCatalog { get; set; }

        
        public DateTime? LastUpdate { get; set; }

        
        public DateTime? Time { get; set; }

        
        public string FlynnRegion { get; set; }

        
        public double? Latitude { get; set; }

        
        public double? Longitude { get; set; }

        
        public double? Depth { get; set; }

        
        public string EventType { get; set; }

        
        public string Author { get; set; }

        
        public double? Magnitude { get; set; }

        
        public string MagnitudeType { get; set; }

        
        public string Unid { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    }
}
