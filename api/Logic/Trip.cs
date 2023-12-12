using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Logic
{
    public class Trip : ITrip
    {
        private readonly DataContext _db;
        public Trip(DataContext db)
        {
            _db = db;
        }
        public TripPlan CreateTripPlan(int accountId)
        {
            var tripPlan = new TripPlan
            {
                AccountId = accountId,
                IsPublic = false,
                Places = new List<TripPlace>()
            };
            Console.WriteLine("dasdas");
            _db.TripPlans.Add(tripPlan);
            _db.SaveChanges();

            return tripPlan;
        }
        public TripPlan GetTripPlan(int tripPlanId)
        {
            var tripPlan = _db.TripPlans.Include(tp => tp.Places).FirstOrDefault(tp => tp.Id == tripPlanId);
            return tripPlan;
        }
        public void AddPlaceToTripPlan(int tripPlanId, int accountId, Place place)
        {
            var tripPlace = new TripPlace
            {
                ApiPlaceId = place.PlaceId,
                TripPlanId = tripPlanId,
                AccountId = accountId
            };

            _db.TripPlaces.Add(tripPlace);
            _db.SaveChanges();
        }
        public void DeleteTripPlan(int tripPlanId)
        {
            var tripPlan = _db.TripPlans.Include(t => t.Places).FirstOrDefault(t => t.Id == tripPlanId);
            if( tripPlan != null )
            {
                _db.TripPlaces.RemoveRange(tripPlan.Places);
                _db.TripPlans.Remove(tripPlan);
                _db.SaveChanges();
            }           
        }
        public void RemovePlaceFromTripPlan(string tripPlaceId, int tripPlanId)
        {
            var tripPlace = _db.TripPlaces.FirstOrDefault(tp => tp.ApiPlaceId == tripPlaceId && tp.TripPlanId == tripPlanId);

            if( tripPlace != null )
            {
                _db.TripPlaces.Remove(tripPlace);
                _db.SaveChanges();
            }
        }
    }
}
