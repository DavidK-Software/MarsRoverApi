using Microsoft.EntityFrameworkCore;

namespace MarsRoverApi.Models
{
    public class MarsRoverDbContext : DbContext
    {
        public MarsRoverDbContext(DbContextOptions<MarsRoverDbContext> options) : base(options)
        {
        }

        public DbSet<MarsPhoto> MarsPhotos { get; set; }
        public DbSet<Rover> Rovers { get; set; }
        public DbSet<Camera> Cameras { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }
    }
}
