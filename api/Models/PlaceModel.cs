using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace api.Models
{
    public class PlaceModel
    {
        [JsonProperty("place_id")]
        public string? PlaceId { get; set; } 
        public string? Name { get; set; }
        public string? Vicinity { get; set; } //adres
        
        public double? Rating { get; set; } // ocena
        [JsonProperty("formatted_phone_number")]
        public string? FormattedPhoneNumber { get; set; }
        public string? Website { get; set; } //

        public List<string>? Types { get; set; } // lista kategorii
    }    
}
