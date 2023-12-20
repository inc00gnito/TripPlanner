using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlacesController : ControllerBase
    {
        private readonly IPlaces _places;
        public PlacesController(IPlaces places)
        {
            _places = places;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlacesByCategory(string category, string placeName, int radius)
        {
            try
            {
                var placesResponse = await _places.GetPlaces(category, placeName, radius);

                if (placesResponse == null)
                {
                    return NotFound();
                }
                var placesWithDetailsResponse = await _places.GetPlaceWithDetails(placesResponse);
                return Ok(placesWithDetailsResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Erorre: {ex.Message}");
            }
        }
        [HttpPost("route")]
        public async Task<IActionResult> GetRouteOfPlaces([FromBody] List<Place> places)
        {
            try
            {
                var routeResponse = await _places.GetRoute(places);
                
                if (routeResponse == null)
                {
                    return NotFound();
                }
                return Ok(routeResponse);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
