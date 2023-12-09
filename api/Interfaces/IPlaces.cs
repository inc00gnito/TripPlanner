using api.Models;

namespace api.Interfaces
{
    public interface IPlaces
    {
        public Task<GooglePlacesResponseModel> GetPlaces(string category, int radius, double latitude, double longitude);
        public Task<List<PlaceModel>> GetPlaceWithDetails(GooglePlacesResponseModel placesResponse);
        public Task<string []> GetRoute(List<PlaceModel> places);
    }
}
