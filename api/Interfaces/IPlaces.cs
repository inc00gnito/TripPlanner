using api.Models;

namespace api.Interfaces
{
    public interface IPlaces
    {
        public Task<GooglePlacesResponse> GetPlaces(string category, int radius, double latitude, double longitude);
        public Task<List<PlaceModel>> GetPlaceWithDetails(GooglePlacesResponse placesResponse);
        public Task<string []> GetRoute(List<PlaceModel> places);
    }
}
