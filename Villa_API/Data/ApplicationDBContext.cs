using Microsoft.EntityFrameworkCore;
using Villa_API.Models;

namespace Villa_API.Data
{
    public class ApplicationDBContext:DbContext
    { 
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(

                new Villa()
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Details = "Lol",
                    ImageUrl = "dwadaw",
                    Occupancy = 5,
                    Rate = 200, 
                    Sqft = 200,  
                    Amenity = "", 
                    CreatedDate = DateTime.Now

                }

                );
        }
    }
}
