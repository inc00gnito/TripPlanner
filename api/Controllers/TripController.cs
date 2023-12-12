using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public class TripController : Controller
    {
        private readonly ITrip _trip;

        public TripController(ITrip trip)
        {
            _trip = trip;
        }

        [HttpPost("create")]
        public IActionResult CreateTripPlanByUser(int accountId)
        {
            var tripPlan = _trip.CreateTripPlan(accountId);
            return Ok(tripPlan);
        }

        [HttpDelete("delete/{tripPlanId}")]
        public IActionResult DeleteTripPlanByUser(int tripPlanId)
        {
            var tripPlan = _trip.GetTripPlan(tripPlanId);
            if(tripPlan != null)
            {
                _trip.DeleteTripPlan(tripPlanId);
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet("get/{tripPlanId}")]
        public IActionResult GetTripPlanByUser(int tripPlanId)
        {
            var tripPlan = _trip.GetTripPlan(tripPlanId);
            return tripPlan != null ? Ok(tripPlan) : NotFound();
        }
        [HttpPost("addPlace")]
        public IActionResult AddPlaceToTripPlanByUser(int tripPlanId, int accountId, Place place)
        {
            _trip.AddPlaceToTripPlan(tripPlanId, accountId, place);
            return Ok();
        }
        [HttpDelete("deletePlace/place/{tripPlaceId}/plan/{tripPlanId}")]
        public IActionResult RemovePlaceFromTripPlanByUser(string tripPlaceId, int tripPlanId) 
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
