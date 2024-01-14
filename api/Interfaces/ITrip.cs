using api.Models;

namespace api.Interfaces
{
    public interface ITrip
    {
        List<TripPlan> GetUserTripPlans(int accountId);
        TripPlan CreateTripPlan(int accountId, string startDate, string endDate);
        TripPlan GetTripPlan(int tripPlanId);
        Task<List<TripPlan>> GetAllPublicTripPlans();
        void AddPlaceToTripPlan(int tripPlanId, int accountId, Place place, string chosenDay);
        void DeleteTripPlan(int tripPlanId);
        void RemovePlaceFromTripPlan(string tripPlaceId, int tripPlanId);
        Task<TripPlan> ShareOrUnsharePlanAsync(int planId, int accountId, bool isPublic);
    }
}
