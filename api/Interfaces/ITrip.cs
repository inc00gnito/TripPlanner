using api.Models;

namespace api.Interfaces
{
    public interface ITrip
    {
        int ReadAccountID(string token);
        TripPlan CreateTripPlan(string token);
        TripPlan GetTripPlan(int tripPlanId);
        void AddPlaceToTripPlan(int tripPlanId, string token, Place place);
        void DeleteTripPlan(int tripPlanId);
        void RemovePlaceFromTripPlan(string tripPlaceId, int tripPlanId);
    }
}
