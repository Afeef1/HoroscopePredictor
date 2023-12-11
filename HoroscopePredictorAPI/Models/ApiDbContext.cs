using Microsoft.EntityFrameworkCore;

namespace HoroscopePredictorAPI.Models
{
    public class ApiDbContext : DbContext
    {

        public ApiDbContext(DbContextOptions<ApiDbContext>options):base(options)
        {
            
        }
        public DbSet<RegisterUser> Users { get; set; }
        public DbSet<HoroscopeData> HoroscopeData { get; set; }
        public DbSet<PredictionData> PredictionData { get; set; }

        public DbSet<UserCacheData> UserCacheData { get; set; }

     
    }
}
