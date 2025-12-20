using Newtonsoft.Json;
using System.Text.Json.Serialization;
using MessagePack;

namespace SeismicEventsFireEvents.DTOs
{
    [MessagePackObject]
    public class SeismicProperties
    {
        [Key(0)]
        [JsonProperty("source_id")]
        public string SourceId { get; set; }

        [Key(1)]
        [JsonProperty("source_catalog")]
        public string SourceCatalog { get; set; }

        [Key(2)]
        [JsonProperty("lastupdate")]
        public DateTime? LastUpdate { get; set; }

        [Key(3)]
        [JsonProperty("time")]
        public DateTime? Time { get; set; }

        [Key(4)]
        [JsonProperty("flynn_region")]
        public string FlynnRegion { get; set; }

        [Key(5)]
        [JsonProperty("lat")]
        public double? Latitude { get; set; }

        [Key(6)]
        [JsonProperty("lon")]
        public double? Longitude { get; set; }

        [Key(7)]
        [JsonProperty("depth")]
        public double? Depth { get; set; }

        [Key(8)]
        [JsonProperty("evtype")]
        public string EventType { get; set; }

        [Key(9)]
        [JsonProperty("auth")]
        public string Author { get; set; }

        [Key(10)]
        [JsonProperty("mag")]
        public double? Magnitude { get; set; }

        [Key(11)]
        [JsonProperty("magtype")]
        public string MagnitudeType { get; set; }

        [Key(12)]
        [JsonProperty("unid")]
        public string Unid { get; set; }

        [Key(13)]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public override bool Equals(object? obj)
        {
            SeismicProperties otherObj = obj as SeismicProperties;
            if (otherObj is null)
            {
                return false;
            }
            return SourceId.Equals(SourceId) &&
                   SourceCatalog.Equals(SourceCatalog) &&
                   LastUpdate.Equals(LastUpdate) &&
                   Time.Equals(Time) &&
                   FlynnRegion.Equals(FlynnRegion) &&
                   Latitude.Equals(Latitude) &&
                   Longitude.Equals(Longitude) &&
                   Depth.Equals(Depth) &&
                   EventType.Equals(EventType) &&
                   Author.Equals(Author) &&
                   Magnitude.Equals(Magnitude) &&
                   MagnitudeType.Equals(MagnitudeType) &&
                   Unid.Equals(Unid);
        }
        public override int GetHashCode()
        {
            return SourceId.GetHashCode() +
                   SourceCatalog.GetHashCode() +
                   LastUpdate.GetHashCode() +
                   Time.GetHashCode() +
                   FlynnRegion.GetHashCode() +
                   Latitude.GetHashCode() +
                   Longitude.GetHashCode() +
                   Depth.GetHashCode() +
                   EventType.GetHashCode() +
                   Author.GetHashCode() +
                   Magnitude.GetHashCode() +
                   MagnitudeType.GetHashCode() +
                   Unid.GetHashCode();
        }
    }
}