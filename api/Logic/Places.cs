﻿using api.Data;
using api.Exceptions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;

namespace api.Logic
{
    public class Places : IPlaces
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly DataContext _db;
        public Places(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<GooglePlacesResponse> GetPlaces(string category, int radius, double latitude, double longitude)
        {
            string apiKey = _configuration ["GooglePlacesApi:ApiKey"];

            string baseUrl = "https://maps.googleapis.com/maps/api/place/nearbysearch/json";

            string requestUrl = $"{baseUrl}?location={latitude},{longitude}&radius={radius}&type={category}&key={apiKey}";

            using (HttpResponseMessage response = await _httpClient.GetAsync(requestUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    var placesResponse = JsonConvert.DeserializeObject<GooglePlacesResponse>(content);

                    return placesResponse;
                }
                else
                {
                    throw new BadRequestException("Cannot get a place");
                }
            }
        }
        public async Task<List<Place>> GetPlaceWithDetails(GooglePlacesResponse placesResponse)
        {
            string apiKey = _configuration ["GooglePlacesApi:ApiKey"];
            string detailsUrl = "https://maps.googleapis.com/maps/api/place/details/json";
            List<Place> places = new List<Place>();

            foreach (var placeWithDetails in placesResponse.Results)
            {
                string requestDetailsUrl = $"{detailsUrl}?place_id={placeWithDetails.PlaceId}&key={apiKey}";

                using (HttpResponseMessage detailsResponse = await _httpClient.GetAsync(requestDetailsUrl))
                {
                    if (detailsResponse.IsSuccessStatusCode)
                    {
                        string detailsContent = await detailsResponse.Content.ReadAsStringAsync();

                        var placeWithDetailsResponse = JsonConvert.DeserializeObject<GooglePlaceResponse>(detailsContent);

                        Place place = new Place()
                        {
                            PlaceId = placeWithDetailsResponse.Result.PlaceId,
                            Name = placeWithDetailsResponse.Result.Name,
                            Rating = placeWithDetailsResponse.Result.Rating,
                            Vicinity = placeWithDetailsResponse.Result.Vicinity,
                            Types = placeWithDetailsResponse.Result.Types.ToList(),
                            Website = placeWithDetailsResponse.Result.Website,
                            FormattedPhoneNumber = placeWithDetailsResponse.Result.FormattedPhoneNumber
                        };
                        places.Add(place);
                    }
                    else
                    {
                        throw new BadRequestException("Cannot get place with details");
                    }
                }          
            }
            return places;
        }
        public async Task<string []> GetRoute(List<Place> places)
        {
            var instructionsList = new List<string>();

            string apiKey = _configuration ["GooglePlacesApi:ApiKey"];

            string waypointsString = string.Join("|", places.Skip(1).Take(places.Count - 2).Select(p => p.PlaceId));

            string apiUrl = $"https://maps.googleapis.com/maps/api/directions/json" +
                        $"?origin=place_id:{Uri.EscapeDataString(places [0].PlaceId)}" +
                        $"&destination=place_id:{Uri.EscapeDataString(places [places.Count - 1].PlaceId)}" +
                        $"&waypoints=place_id:{Uri.EscapeDataString(waypointsString)}" +
                        $"&key={apiKey}" +
                        $"&mode=driving";
      
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var route = ParseDirections(content);
                return route;
            }
            else 
            {
                throw new BadRequestException("Cannot get route");
            }
        }
        private static string[] ParseDirections(string json)
        {
            var routes = JToken.Parse(json)? ["routes"]?.SelectMany(route =>
                    route? ["legs"]?.SelectMany(leg =>
                        leg? ["steps"]?.Select(step =>
                        {
                            var htmlInstructions = (string)step ["html_instructions"];
                            var travelMode = (string)step ["travel_mode"];
                            var duration = (string)step ["duration"]? ["text"];
                            var distance = (string)step ["distance"]? ["text"];

                            return $"{htmlInstructions} ({travelMode}, {duration}, {distance})";
                        })));
            
            return routes.ToArray();     
        }
        public async Location GeocodeLocation(string placeName)
        {
            string apiKey = _configuration ["GooglePlacesApi:ApiKey"];
            using( var httpClient = new HttpClient() )
            {
                var apiUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={placeName}&key={apiKey}";

                var response = await httpClient.GetStringAsync(apiUrl);
                var geocodingResponse = JsonConvert.DeserializeObject<Geometry>(response);

                if( geocodingResponse.Location != null)
                {
                    var location = geocodingResponse.Location;
                    return location;
                }
                else
                {
                    throw new BadRequestException("Cannot get place location");
                }
            }
        }
    }
}
