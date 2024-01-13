using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace api.Models
{
    public class Place
    {
        [JsonProperty("place_id")]
        public string? PlaceId { get; set; } 
        public string? Name { get; set; }
        public string? Vicinity { get; set; }
        
        public double? Rating { get; set; }
        [JsonProperty("formatted_phone_number")]
        public string? FormattedPhoneNumber { get; set; }
        public string? Website { get; set; }

        public List<string>? Types { get; set; }

        [JsonProperty("opening_hours")]
        public PlaceOpeningHours Opening_Hours { get; set; }
    }
}
