using CitiesManager.Core.Entities;
using CitiesManager.Core.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace CitiesManager.Infrastructure.DatabaseContext
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        { }

        public ApplicationDBContext() { }

        public virtual DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Data Seeding
            modelBuilder.Entity<City>().HasData(new City() { CityID =Guid.Parse("15A51043-B1C8-401A-B751-36119876BDC6"), CityName="USA"});
            modelBuilder.Entity<City>().HasData(new City() { CityID =Guid.Parse("A1820C9E-AEFF-4629-A940-6C2712C83DFE"),CityName = "JAPAN"});
        }
    }
}
