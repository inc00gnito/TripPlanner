using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class TripPlan
    {
        [Key]
        public int Id { get; set; }
        public string AccountId { get; set; }
        public bool IsPublic { get; set; }
        public List<TripPlace> Places { get; set; }
    }
}
