using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

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
        public List<TripPlan> GetUserTripPlans(int accountId) 
        {
            var tripPlans = _db.TripPlans.Where(t=>t.AccountId==accountId).Include(t=>t.Places).ToList();
            return tripPlans;
        }
        public TripPlan GetTripPlan(int tripPlanId)
        {
            var tripPlan = _db.TripPlans.Include(tp => tp.Places).FirstOrDefault(tp => tp.Id == tripPlanId);
            return tripPlan;
        }
        public void AddPlaceToTripPlan(int tripPlanId, int accountId, string placeId)
        {
            var tripPlace = new TripPlace
            {
                ApiPlaceId = placeId,
                TripPlanId = tripPlanId,               
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

        public async Task<TripPlan> SharePlanAsync(int planId, int accountId)
        {
            TripPlan tripPlan =  await _db.TripPlans.Where(e => e.AccountId == accountId).FirstOrDefaultAsync(e => e.Id == planId);
            if (tripPlan != null)
            {
                tripPlan.IsPublic = true;
                _db.TripPlans.Update(tripPlan);
                _db.SaveChangesAsync();
            }
            return tripPlan;
        }
    }
}
