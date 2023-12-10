using api.Models;

namespace api.Interfaces
{
    public interface IPlaces
    {
        public Task<GooglePlacesResponse> GetPlaces(string category, int radius, double latitude, double longitude);
        public Task<List<Place>> GetPlaceWithDetails(GooglePlacesResponse placesResponse);
        public Task<string []> GetRoute(List<Place> places);
    }
}
