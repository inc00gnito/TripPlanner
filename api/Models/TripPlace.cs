using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class TripPlace
    {
        [Key]
        public int Id { get; set; }
        public string ApiPlaceId { get; set; }
        [ForeignKey("TripPlan")]
        public int TripPlanId { get; set; }
        public DateTime ChosenDay { get; set; }
        [ForeignKey("Account")]
        public TripPlan TripPlan;
    }
}
