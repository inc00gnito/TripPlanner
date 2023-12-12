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
            if(tripPlan != null)
            {
                _trip.DeleteTripPlan(tripPlanId);
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet("{tripPlanId}")]
        public IActionResult GetTripPlan(int tripPlanId)
        {
            var tripPlan = _trip.GetTripPlan(tripPlanId);
            return tripPlan != null ? Ok(tripPlan) : NotFound();
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
            if(tripPlan != null) 
            {
                _trip.RemovePlaceFromTripPlan(tripPlaceId, tripPlanId);
                return Ok();
            }
            return NotFound();         
        }
    }
}
