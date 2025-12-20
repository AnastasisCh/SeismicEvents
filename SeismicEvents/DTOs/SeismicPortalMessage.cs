using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SeismicEventsFireEvents.DTOs
{
    public class SeismicPortalMessage
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("data")]
        public SeismicData Data { get; set; }

        public override bool Equals(object? obj)
        {
            SeismicPortalMessage otherObj = obj as SeismicPortalMessage;
            if (otherObj is null)
                return false;
            return Data == otherObj.Data;
        }
        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }
    }
}
