namespace api.Models
{
    public class GooglePlacesResponseModel
    {
        public string Status { get; set; }
        public List<string>? HtmlAttributions { get; set; }
        public List<PlaceModel>? Results { get; set; }
    }
}
