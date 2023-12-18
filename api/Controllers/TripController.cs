﻿using api.Interfaces;
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
            return tripPlans == null ? Ok(tripPlans) : NotFound(tripPlans);
           
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
                return Ok(tripPlan);
            }
            return NotFound("Trip plan not found");
        }
        [HttpGet("{tripPlanId}")]
        public IActionResult GetTripPlan(int tripPlanId)
        {
            var tripPlan = _trip.GetTripPlan(tripPlanId);
            return tripPlan != null ? Ok(tripPlan) : NotFound(tripPlan);
        }
        [HttpPost("addPlace")]
        public IActionResult AddPlaceToTripPlan(int tripPlanId, string placeId)
        {
            var accountId = Convert.ToInt32(User.Claims.First(x => x.Type == "id").Value);
            _trip.AddPlaceToTripPlan(tripPlanId, accountId, placeId);
            return Ok("A new place has been successfully added");
        }
        [HttpDelete("place/{tripPlaceId}/plan/{tripPlanId}")]
        public IActionResult RemovePlaceFromTripPlan(string tripPlaceId, int tripPlanId) 
        {
            var tripPlan = _trip.GetTripPlan(tripPlanId);
            if(tripPlan != null) 
            {
                _trip.RemovePlaceFromTripPlan(tripPlaceId, tripPlanId);
                return Ok("Removed place from plan");
            }
            return NotFound("Place not found");         
        }

        [HttpGet("share/{tripPlanId}")]
        public async Task<IActionResult> SharePlanAsync(int tripPlanId)
        {
            var accountId = Convert.ToInt32(User.Claims.First(x => x.Type == "id").Value);

            TripPlan tripPlan = await _trip.SharePlanAsync(tripPlanId, accountId);
            if (tripPlan == null)
            {
                return NotFound("Not Found");
            }
            return Ok("Plan was shared");
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
    }
}
