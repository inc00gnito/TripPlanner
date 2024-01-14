using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
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
        public void savePlaceToDataBase(string placeId, int tripPlanId)
        {
            var tripPlace = new TripPlace
            {
                ApiPlaceId = placeId,
                TripPlanId = tripPlanId,
            };
            _db.TripPlaces.Add(tripPlace);
            _db.SaveChanges();
        }
        public void AddPlaceToTripPlan(int tripPlanId, int accountId, Place place, string chosenDay)
        {
            DateTime dateTime = DateTime.Parse(chosenDay);
            DayOfWeek day = dateTime.DayOfWeek;
            string[] tabbleOfWeekDays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            int dayAsNumber = Array.IndexOf(tabbleOfWeekDays, day.ToString());

            int hour = dateTime.Hour;
            int minute = dateTime.Minute;
            int requestedHour = Convert.ToInt32(hour.ToString() + minute.ToString());
            if (minute < 10)
            {
                requestedHour *= 10;
            }

            if (place.Opening_Hours.periods[0].Close.Equals(null)){
                savePlaceToDataBase(place.PlaceId, tripPlanId);
                return;
            }

            for (int i = 0; i <= 6; i++)
            {               
                if (dayAsNumber == place.Opening_Hours.periods[i].Open.Day)
                {
                    PlaceOpeningHoursPeriodDetails dayClose = place.Opening_Hours.periods[i].Close;
                    PlaceOpeningHoursPeriodDetails dayOpen = place.Opening_Hours.periods[i].Open;
                    int openTime = Convert.ToInt32(dayOpen.Time);
                    int closeTime = Convert.ToInt32(dayClose.Time);
                    if (dayClose.Day == dayOpen.Day)
                    {
                        
                        if(openTime<requestedHour && requestedHour < closeTime)
                        {
                            savePlaceToDataBase(place.PlaceId, tripPlanId);
                            return;
                        }
                    }else
                    {
                        if (requestedHour > openTime)
                        {
                            savePlaceToDataBase(place.PlaceId, tripPlanId);
                            return;
                        }
                    }
                }else if(dayAsNumber == place.Opening_Hours.periods[i].Close.Day)
                {
                    PlaceOpeningHoursPeriodDetails dayClose = place.Opening_Hours.periods[i].Close;
                    int closeTime = Convert.ToInt32(dayClose.Time);

                    if (closeTime > requestedHour)
                    {
                        savePlaceToDataBase(place.PlaceId, tripPlanId);
                        return;
                    }
                }
            }
            throw new Exception("At the selected hour the local is closed.");       
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
    }
}
