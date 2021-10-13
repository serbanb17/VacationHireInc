// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.EntityFrameworkCore;
using VacationHireInc.DataLayer.Models;
using VacationHireInc.DataLayer.Initializers;
using VacationHireInc.DataLayer.Configurations;

namespace VacationHireInc.DataLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        { 
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<VehicleOrder> VehicleOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleOrderConfiguration());

            modelBuilder.InitializeUsers();
            modelBuilder.InitializeCustomers();
            modelBuilder.InitializeVehicles();
            modelBuilder.InitializeVehicleOrders();
        }
    }
}
