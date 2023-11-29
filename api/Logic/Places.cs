using api.Interfaces;
using api.Models;
using Newtonsoft.Json;
using System.Net.Http;

namespace api.Logic
{
    public class Places : IPlaces
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public Places(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<GooglePlacesResponse> GetPlaces(string category, int radius, double latitude, double longitude)
        {
            string apiKey = _configuration ["GooglePlacesApi:ApiKey"];

            string baseUrl = "https://maps.googleapis.com/maps/api/place/nearbysearch/json";
            

            string requestUrl = $"{baseUrl}?location={latitude},{longitude}&radius={radius}&type={category}&key={apiKey}";


            using( HttpResponseMessage response = await _httpClient.GetAsync(requestUrl))
            {
                if( response.IsSuccessStatusCode )
                {
                    string content = await response.Content.ReadAsStringAsync();

                    var placesResponse = JsonConvert.DeserializeObject<GooglePlacesResponse>(content);                 

                    return placesResponse;
                }
                else 
                {
                    return null;
                }
            }              
        }
        public async Task<List<PlaceModel>> GetPlaceWithDetails(GooglePlacesResponse placesResponse)
        {
            string apiKey = _configuration ["GooglePlacesApi:ApiKey"];
            string detailsUrl = "https://maps.googleapis.com/maps/api/place/details/json";
            List<PlaceModel> places = new List<PlaceModel>();

            foreach( var placeWithDetails in placesResponse.Results )
            {

                string requestDetailsUrl = $"{detailsUrl}?place_id={placeWithDetails.Place_id}&key={apiKey}";

                HttpResponseMessage detailsResponse = await _httpClient.GetAsync(requestDetailsUrl);

                string detailsContent = await detailsResponse.Content.ReadAsStringAsync();

                var placeWithDetailsResponse = JsonConvert.DeserializeObject<GooglePlaceResponse>(detailsContent);

                PlaceModel place = new PlaceModel()
                {
                    Place_id = placeWithDetailsResponse.Result.Place_id,
                    Name = placeWithDetailsResponse.Result.Name,
                    Rating = placeWithDetailsResponse.Result.Rating,
                    Vicinity = placeWithDetailsResponse.Result.Vicinity,
                    Types = placeWithDetailsResponse.Result.Types.ToList(),
                    Website = placeWithDetailsResponse.Result.Website,
                    Formatted_phone_number = placeWithDetailsResponse.Result.Formatted_phone_number
                };
                places.Add(place);
            }
            return places;
        }

        
    }
}
