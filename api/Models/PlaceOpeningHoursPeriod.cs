using Newtonsoft.Json;

namespace api.Models
{
    
    public class PlaceOpeningHoursPeriod
    {
        [JsonProperty("open")]
        public PlaceOpeningHoursPeriodDetails? Open { get; set; }

        [JsonProperty("close")]
        public PlaceOpeningHoursPeriodDetails? Close { get; set; }
    }
}
