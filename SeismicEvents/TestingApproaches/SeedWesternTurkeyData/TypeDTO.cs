using SeismicEventsFireEvents.DTOs;
using System.Text.Json.Serialization;

namespace SeismicEventsFireEvents.TestingApproaches.SeedWesternTurkeyData;

public class TypeDTO
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("geometry")]
    public object Geometry { get; set; }
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("properties")]
    public DTOs.SeismicProperties Properties { get; set; }
}