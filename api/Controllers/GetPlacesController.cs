using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetPlacesController : ControllerBase
    {
        private readonly IPlaces _places;

        public GetPlacesController(IPlaces places)
        {
            _places = places;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlaces(string category, int radius, double latitude, double longitude)
        {
            try
            {
                var placesResponse = await _places.GetPlaces(category, radius, latitude, longitude);
                //
                if( placesResponse == null )
                {
                    return NotFound();
                }
                var placesWithDetailsResponse = await _places.GetPlaceWithDetails(placesResponse);
                return Ok(placesWithDetailsResponse);
            }
            catch( Exception ex )
            {
                return StatusCode(500, $"Internal Server Erorre: {ex.Message}");
            }
        }
        [HttpPost("route")]
        public async Task<IActionResult> GetRoutePlaces([FromBody] List<PlaceModel> places)
        {
            try
            {
                var routeResponse = await _places.GetRoute( places );
                if( routeResponse == null )
                {
                    return NotFound();
                }
                return Ok(routeResponse);
            }
            catch( Exception ex ) 
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
