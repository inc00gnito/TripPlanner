using api.Models;

namespace api.Interfaces
{
    public interface IPlaces
    {
        public Task<GooglePlacesResponse> GetPlaces(string category, string placeName, int radius);
        public Task<List<Place>> GetPlaceWithDetails(GooglePlacesResponse placesResponse);
        public Task<string []> GetRoute(List<Place> places);
        public Task<Location> GeocodeLocation(string placeName);
    }
}
