using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class TripController : Controller
    {
        private readonly ITrip _trip;
       
        public TripController(ITrip trip)
        {
            _trip = trip;
        }
        [HttpGet("all")]
        public IActionResult GetAllUserPlans()
        {
            var accountId = Convert.ToInt32(User.Claims.First(x => x.Type == "id").Value);
            var tripPlans = _trip.GetUserTripPlans(accountId);
            return tripPlans != null ? Ok(tripPlans) : NotFound("Trip plan not found");
           
        }
        [HttpPost("create")]
        public IActionResult CreateTripPlan()
        {
            var accountId = Convert.ToInt32(User.Claims.First(x => x.Type == "id").Value);
            var tripPlan = _trip.CreateTripPlan(accountId);
            return Ok(tripPlan);
        }

        [HttpDelete("{tripPlanId}")]
        public IActionResult DeleteTripPlan(int tripPlanId)
        {
            var tripPlan = _trip.GetTripPlan(tripPlanId);
            if(tripPlan == null)
            {
                return NotFound("Plan not found");         
            }
            _trip.DeleteTripPlan(tripPlanId);
            return Ok();
        }
        [HttpGet("{tripPlanId}")]
        public IActionResult GetTripPlan(int tripPlanId)
        {
            var tripPlan = _trip.GetTripPlan(tripPlanId);
            return tripPlan != null ? Ok(tripPlan) : NotFound("Trip plan not found");
        }
        [HttpPost("addPlace")]
        public IActionResult AddPlaceToTripPlan(int tripPlanId, string placeId)
        {
            var accountId = Convert.ToInt32(User.Claims.First(x => x.Type == "id").Value);
            _trip.AddPlaceToTripPlan(tripPlanId, accountId, placeId);
            return Ok();
        }
        [HttpDelete("place/{tripPlaceId}/plan/{tripPlanId}")]
        public IActionResult RemovePlaceFromTripPlan(string tripPlaceId, int tripPlanId) 
        {
            var tripPlan = _trip.GetTripPlan(tripPlanId);
            if(tripPlan == null) 
            {
                return NotFound("Trip plan not found");                
            }
            _trip.RemovePlaceFromTripPlan(tripPlaceId, tripPlanId);
            return Ok();
        }
    }
}
