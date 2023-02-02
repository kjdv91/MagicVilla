using Microsoft.EntityFrameworkCore;
using Village_API.Models;

namespace Village_API.Datos
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                    new Villa()
                    {
                        Id = 1,
                        Name = "Villa Real",
                        Details = "Casa en ciudadela privada",
                        ImageUrl = "",
                        Capacity = 4,
                        SquareMetters = 125.00,
                        Fee = 120,
                        EmitionCreated = DateTime.Now,
                        UpdateDate = DateTime.Now


                    });
        }

    }
}
