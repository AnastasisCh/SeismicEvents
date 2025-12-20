using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SeismicEventsFireEvents.DTOs
{
    public class SeismicGeometry
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        // [longitude, latitude, depth]
        [JsonProperty("coordinates")]
        public List<double> Coordinates { get; set; }
        public override bool Equals(object? obj)
        {
            SeismicGeometry seismicGeometry = obj as SeismicGeometry;
            if (seismicGeometry is null)
            {
                return false;
            }
            return Type.Equals(Type) &&
                   Coordinates.SequenceEqual(Coordinates);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Coordinates);
        }
    }
}
