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
        public Places(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async List<GooglePlacesResponse> GetPlaces(string category, int radius, double latitude, double longitude)
        {
            string apiKey = _configuration ["GooglePlacesApi:ApiKey"];

            string baseUrl = "https://maps.googleapis.com/maps/api/place/nearbysearch/json";
            string detailsUrl = "https://maps.googleapis.com/maps/api/place/details/json";

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
                    return response.StatusCode;
                }
            }

            
        }
    }
}
