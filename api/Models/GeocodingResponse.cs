using Newtonsoft.Json;

namespace api.Models
{
    public class GeocodingResponse
    {
        [JsonProperty("results")]
        public Results[] Result { get; set; }

        public class Results
        {
            public Geometry Geometry { get; set; }
         
        }
    }

}
