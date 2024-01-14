using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections;

namespace api.Models
{
    public class PlaceOpeningHours
    {

        [JsonProperty("open_now")]
        public bool isOpen { get; set; }

        [JsonProperty("periods")]
        public List<PlaceOpeningHoursPeriod> periods { get; set; }  
    }
}


    
