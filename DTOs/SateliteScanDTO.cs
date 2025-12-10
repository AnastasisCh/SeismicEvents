using System.Text.Json.Serialization;

namespace SeismicEventsFireEvents.DTOs;

public class SateliteScanDTO
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("bright_ti4")]
    public double BrightTi4 { get; set; }

    [JsonPropertyName("scan")]
    public double Scan { get; set; }

    [JsonPropertyName("track")]
    public double Track { get; set; }

    [JsonPropertyName("acq_date")]
    public string AcqDate { get; set; }

    [JsonPropertyName("acq_time")]
    public string AcqTime { get; set; }

    [JsonPropertyName("satellite")]
    public string Satellite { get; set; }

    [JsonPropertyName("instrument")]
    public string Instrument { get; set; }
    private string _confidence;

    [JsonPropertyName("confidence")]
    public string Confidence
    {
        get => _confidence;
        set
        {
            if (Instrument.Equals("MODIS"))
            {
                if (float.TryParse(value, out float confValue))
                {
                    if (confValue >= 75f)
                        _confidence = "h";
                    else if (confValue >= 30)
                        _confidence = "n";
                    else
                        _confidence = "l";
                }
            }
            else
            {
                _confidence = value;
            }
        }
    }

    [JsonPropertyName("version")]
    public string Version { get; set; }

    [JsonPropertyName("bright_ti5")]
    public double BrightTi5 { get; set; }

    [JsonPropertyName("frp")]
    public double Frp { get; set; }

    [JsonPropertyName("day_night")]
    public string DayNight { get; set; }

    public override bool Equals(object? obj)
    {
        SateliteScanDTO? otherObj = obj as SateliteScanDTO;
        if (otherObj == null) return false;
        return Latitude == otherObj.Latitude && Longitude == otherObj.Longitude;
    }
    public override int GetHashCode()
    {
        return Latitude.GetHashCode() ^ Longitude.GetHashCode();
    }
}
