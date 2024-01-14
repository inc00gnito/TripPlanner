using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace api.Models
{
    public class PlaceOpeningHoursPeriodDetails
    {
        [JsonProperty("day")]
        public int? Day {get; set;}

        [JsonProperty("time")]
        public string? Time { get; set; }
    }
}