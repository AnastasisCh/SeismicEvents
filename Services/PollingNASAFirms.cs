using SeismicEventsFireEvents.DTOs;
using System.Text.Json;

namespace SeismicEventsFireEvents.Services
{
    public class PollingNASAFirms : BackgroundService
    {
        private readonly string _nasaFirmBaseApiUrl = "https://firms.modaps.eosdis.nasa.gov/api/area/csv/";
        private readonly IConfiguration _configuration;
        private byte _dayRange = 1;
        private readonly HttpClient _firmsClient;
        private readonly HashSet<SateliteScanDTO> _scanDTOs;
        public PollingNASAFirms(IConfiguration configuration)
        {
            _configuration = configuration;
            _firmsClient = new HttpClient();
            _scanDTOs = new HashSet<SateliteScanDTO>();
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response;
            HashSet<string> csvlinessan = new HashSet<string>();
            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var sensor in Enum.GetValues(typeof(SateliteSensors)))
                {
                    response = await _firmsClient.GetAsync(
                    string.Concat(_nasaFirmBaseApiUrl, _configuration.GetValue<string>("NASAFIRMSMapKey"), '/', sensor.ToString(), '/', _configuration.GetValue<string>("GreeceCoordinates"), '/', _dayRange.ToString())
                    );
                    string responseCSV = await response.Content.ReadAsStringAsync();
                    string[] csvLines = responseCSV.Split('\n');
                    foreach (string line in csvLines)
                    {
                        SateliteScanDTO? sateliteScanDTO = ParseCsvLineToSateliteScanDTO(line);
                        string? model = sateliteScanDTO?.Instrument;
                        if (string.IsNullOrEmpty(model) || sateliteScanDTO is null) continue;
                        //exclude low confidence fires
                        if (sateliteScanDTO.Confidence.Equals("l")) continue;
                        if (!_scanDTOs.Contains(sateliteScanDTO) || sateliteScanDTO.Confidence.Equals("h"))
                        {
                            _scanDTOs.Remove(sateliteScanDTO);
                            _scanDTOs.Add(sateliteScanDTO);
                            csvlinessan.Add(line);
                            Console.Write("new fire detected from \n");
                        }

                    }
                }
                await Task.Delay(10000);

                foreach (var scan in _scanDTOs)
                {
                    Console.WriteLine(JsonSerializer.Serialize(scan));
                }
                foreach (var line in csvlinessan)
                {
                    Console.WriteLine(line);
                }
                Console.WriteLine(_scanDTOs.Count() + " " + csvlinessan.Count());
            }
        }
        private SateliteScanDTO ParseCsvLineToSateliteScanDTO(string csvLine)
        {
            string[] values = csvLine.Split(',');
            if (values.Length < 13 || values[0] == "latitude") return null; // Skip header or invalid lines
            return new SateliteScanDTO
            {
                Latitude = double.Parse(values[0]),
                Longitude = double.Parse(values[1]),
                BrightTi4 = double.Parse(values[2]),
                Scan = double.Parse(values[3]),
                Track = double.Parse(values[4]),
                AcqDate = values[5],
                AcqTime = values[6],
                Satellite = values[7],
                Instrument = values[8],
                Confidence = values[9],
                Version = values[10],
                BrightTi5 = double.Parse(values[11]),
                Frp = double.Parse(values[12]),
                DayNight = values.Length > 13 ? values[13] : string.Empty
            };
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.Write(cancellationToken.ToString());
        }
    }
    public enum SateliteSensors
    {
        MODIS_NRT = 1,
        VIIRS_NOAA20_NRT = 2,
        VIIRS_NOAA21_NRT = 4,
        VIIRS_SNPP_NRT = 8
    }
}
