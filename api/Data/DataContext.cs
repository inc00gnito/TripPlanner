using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class DataContext :DbContext
    {
        public DataContext(DbContextOptions<DataContext> opt) : base(opt){}
      
        public DbSet<Account> Accounts { get; set; }
        public DbSet<TripPlan> TripPlans { get; set; }
        public DbSet<TripPlace> TripPlaces { get; set; }
    }
}
