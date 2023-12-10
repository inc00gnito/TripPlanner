namespace api.Models
{
    public class GooglePlacesResponse
    {
        public string Status { get; set; }
        public List<string>? HtmlAttributions { get; set; }
        public List<Place>? Results { get; set; }
    }
}
