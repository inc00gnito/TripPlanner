using api.Models;

namespace api.Interfaces
{
    public interface ITrip
    {
        List<TripPlan> GetUserTripPlans(int accountId);
        TripPlan CreateTripPlan(int accountId, DateTime startDate, DateTime endDate);
        TripPlan GetTripPlan(int tripPlanId);
        void AddPlaceToTripPlan(int tripPlanId, int accountId, string placeId);
        void DeleteTripPlan(int tripPlanId);
        void RemovePlaceFromTripPlan(string tripPlaceId, int tripPlanId);
    }
}
