using api.Models;

namespace api.Interfaces
{
    public interface ITrip
    {
        TripPlan CreateTripPlan(int accountId);
        TripPlan GetTripPlan(int tripPlanId);
        void AddPlaceToTripPlan(int tripPlanId, int accountId, Place place);
        void DeleteTripPlan(int tripPlanId);
        void RemovePlaceFromTripPlan(string tripPlaceId, int tripPlanId);
    }
}
