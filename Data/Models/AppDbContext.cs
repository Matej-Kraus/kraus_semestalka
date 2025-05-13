using Microsoft.EntityFrameworkCore;
using kraus_semestalka.Data.Models;

namespace kraus_semestalka.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Recordings> Recordings { get; set; }
        public DbSet<DriveData> DriveData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=147.230.21.212;Initial Catalog=VAPW_PS_Drives;User ID=vapw;Password=cv1k0;TrustServerCertificate=True");
        }
    }
}
