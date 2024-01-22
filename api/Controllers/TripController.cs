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
        private readonly IPlaces _place;
       
        public TripController(ITrip trip, IPlaces place)
        {
            _trip = trip;
            _place = place;
        }
        [HttpGet("all")]
        public IActionResult GetAllUserPlans()
        {
            var accountId = Convert.ToInt32(User.Claims.First(x => x.Type == "id").Value);
            var tripPlans = _trip.GetUserTripPlans(accountId);
            return tripPlans != null ? Ok(tripPlans) : NotFound("Trip plan not found");
           
        }
        [HttpPost("create")]
        public IActionResult CreateTripPlan(string startDate, string endDate, string destination)
        {
            var accountId = Convert.ToInt32(User.Claims.First(x => x.Type == "id").Value);
            var tripPlan = _trip.CreateTripPlan(accountId, destination, startDate, endDate);
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
        public IActionResult AddPlaceToTripPlan(int tripPlanId, string placeId,string chosenDate)
        {
            var accountId = Convert.ToInt32(User.Claims.First(x => x.Type == "id").Value);
            var place = _place.GetPlaceByPlaceID(placeId);
            _trip.AddPlaceToTripPlan(tripPlanId, accountId, place.Result ,chosenDate );
            return Ok(place.Result);
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

        [HttpPut("share/{tripPlanId}/{isPublic}")]
        public async Task<IActionResult> ShareOrUnsharePlanAsync(int tripPlanId, bool isPublic)
        {
            var accountId = Convert.ToInt32(User.Claims.First(x => x.Type == "id").Value);

            TripPlan tripPlan = await _trip.ShareOrUnsharePlanAsync(tripPlanId, accountId, isPublic);
            if (tripPlan == null)
            {
                return NotFound("Not Found");
            }
            else if (tripPlan.IsPublic)
            {
                return Ok("Plan was shared");
            }
            else
            {
                return Ok("Plan was unshared");
            }
        }

        [HttpGet("show/{tripPlanId}")]
        [AllowAnonymous]
        public IActionResult ShowPlan(int tripPlanId)
        {
            var tripPlan = _trip.GetTripPlan(tripPlanId);

            if (tripPlan == null)
            {
                return NotFound("Not Found");
            }
            else if (tripPlan.IsPublic == false)
            {
                return Ok("Plan is not shared");
            }

            return Ok(tripPlan);
        }
        [HttpGet("showPublicTripPlans")]
        [AllowAnonymous]
        public async Task<IActionResult> ShowPublicTripPlans()
        {
            var tripPlans = await _trip.GetAllPublicTripPlans();
            if(tripPlans == null)
            {
                return NotFound("Not found list of public plans");
            }
            else
            {
                return Ok(tripPlans);
            }
        }
    }
}
