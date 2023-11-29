namespace api.Models
{
    public class GooglePlaceResponse
    {
        public string Status { get; set; }
        public List<string>? HtmlAttributions { get; set; }
        public PlaceModel Result { get; set; }

    }
}
