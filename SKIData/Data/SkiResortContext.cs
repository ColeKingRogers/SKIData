using Microsoft.EntityFrameworkCore;

namespace SKIData.Data
{
    public class SkiResortContext :DbContext
    {
        public SkiResortContext(DbContextOptions<SkiResortContext> options) : base(options)
        {
        }
        public DbSet<Model.SkiResort> SkiResorts { get; set; }
    }
}
