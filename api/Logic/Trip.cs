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
        public TripPlan CreateTripPlan(int accountId, string startDateJson, string endDateJson)
        {
            DateTime startDate = DeserializeJsonDate(startDateJson);
            DateTime endDate = DeserializeJsonDate(endDateJson);
            if(startDate>endDate)
            {
                throw new Exception("Start day of trip cannot be greater than the end date");
            }
            var tripPlan = new TripPlan
            {
                AccountId = accountId,
                IsPublic = false,
                StartDate = startDate,
                EndDate = endDate,
                Places = new List<TripPlace>()
            };          
            _db.TripPlans.Add(tripPlan);
            _db.SaveChanges();

            return tripPlan;
        }
        public List<TripPlan> GetUserTripPlans(int accountId)
        {
            var tripPlans = _db.TripPlans.Where(t => t.AccountId == accountId).Include(t => t.Places).ToList();
            return tripPlans;
        }
        public TripPlan GetTripPlan(int tripPlanId)
        {
            var tripPlan = _db.TripPlans.Include(tp => tp.Places).FirstOrDefault(tp => tp.Id == tripPlanId);
            return tripPlan;
        }
        public List<TripPlan> GetAllPublicTripPlans()
        {
            var tripPlans = _db.TripPlans.Include(tps => tps.Places).Where(t => t.IsPublic == true).ToList();
            return tripPlans;
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
            if(tripPlan != null)
            {
                _db.TripPlaces.RemoveRange(tripPlan.Places);
                _db.TripPlans.Remove(tripPlan);
                _db.SaveChanges();
            }
        }
        public void RemovePlaceFromTripPlan(string tripPlaceId, int tripPlanId)
        {
            var tripPlace = _db.TripPlaces.FirstOrDefault(tp => tp.ApiPlaceId == tripPlaceId && tp.TripPlanId == tripPlanId);

            if(tripPlace != null)
            {
                _db.TripPlaces.Remove(tripPlace);
                _db.SaveChanges();
            }
        }
        static DateTime DeserializeJsonDate(string jsonDate)
        {            
            jsonDate = jsonDate.Trim('"');
            DateTime result;
            if(DateTime.TryParse(jsonDate, out result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException($"Cannot convert JSON to date: {jsonDate}");
            }
        }

        public async Task<TripPlan> ShareOrUnsharePlanAsync(int planId, int accountId, bool isPublic)
        {
            TripPlan tripPlan = await _db.TripPlans.Where(e => e.AccountId == accountId).FirstOrDefaultAsync(e => e.Id == planId);
            if (tripPlan != null)
            {
                tripPlan.IsPublic = isPublic;
                _db.TripPlans.Update(tripPlan);
                _db.SaveChangesAsync();
            }
            return tripPlan;
        }
    }
}
