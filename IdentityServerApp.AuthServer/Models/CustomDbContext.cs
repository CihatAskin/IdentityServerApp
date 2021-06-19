using Microsoft.EntityFrameworkCore;

namespace IdentityServerApp.AuthServer.Models
{
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions<CustomDbContext> opts) : base(opts)
        {

        }
        public DbSet<CustomUser> CustomUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomUser>().HasData(new CustomUser() { 
                Id = 1, Email = "yan@askin.com", Password = "Pass123",City="İzmir",UserName="Yan"
            });
            
            modelBuilder.Entity<CustomUser>().HasData(new CustomUser() { 
                Id = 2, Email = "can@askin.com", Password = "Pass123",City="Ankara",UserName="Can"
            });
            
            modelBuilder.Entity<CustomUser>().HasData(new CustomUser() { 
                Id = 3, Email = "dan@askin.com", Password = "Pass123",City="Bursa",UserName="Dan"
            });
            
            modelBuilder.Entity<CustomUser>().HasData(new CustomUser() { 
                Id = 4, Email = "man@askin.com", Password = "Pass123",City="Van",UserName="Man"
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
