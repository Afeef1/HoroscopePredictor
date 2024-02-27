using Microsoft.EntityFrameworkCore;

namespace HoroscopePredictorAPI.Models
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext()
        {
            
        }
        public ApiDbContext(DbContextOptions<ApiDbContext>options):base(options)
        {
            
        }
        public virtual DbSet<RegisterUser> Users { get; set; }
        public virtual DbSet<HoroscopeData> HoroscopeData { get; set; }
        public virtual DbSet<PredictionData> PredictionData { get; set; }

        public virtual DbSet<UserCacheData> UserCacheData { get; set; }

     
    }
}
