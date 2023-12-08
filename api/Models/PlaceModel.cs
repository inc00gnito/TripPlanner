using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace api.Models
{
    public class PlaceModel
    {
        public string? Place_id { get; set; } 
        public string? Name { get; set; }
        public string? Vicinity { get; set; } //adres
        
        public double? Rating { get; set; } // ocena
        public string? Formatted_phone_number { get; set; }
        public string? Website { get; set; } //

        public List<string>? Types { get; set; } // lista kategorii
    }
    
    public class GooglePlacesResponse
    {
        public string Status { get; set; }
        public List<string>? HtmlAttributions { get; set; }
        public List<PlaceModel>? Results { get; set; }
    }

   
}
