using Microsoft.EntityFrameworkCore;
using PowerPlantAPITask.Models;

namespace PowerPlantAPITask.Data
{
    public class PowerPlantDbContext : DbContext
    {
        public PowerPlantDbContext(DbContextOptions<PowerPlantDbContext> options) : base(options)
        {
        }

        public DbSet<PowerPlant> PowerPlants { get; set; }
    }
}
