using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SeismicEventsFireEvents.DTOs
{
    public class SeismicData
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("geometry")]
        public SeismicGeometry Geometry { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("properties")]
        public SeismicProperties Properties { get; set; }
        public override bool Equals(object? obj)
        {
            SeismicData otherObj = obj as SeismicData;
            if (otherObj is null)
            {
                return false;
            }
            return Type.Equals(Type) &&
                   Geometry.Equals(Geometry) &&
                   Id.Equals(Id) &&
                   Properties.Equals(Properties);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Geometry, Id, Properties);
        }
    }
}
