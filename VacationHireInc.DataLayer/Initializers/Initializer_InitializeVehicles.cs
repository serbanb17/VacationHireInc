// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.EntityFrameworkCore;
using System;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.DataLayer.Initializers
{
    internal static partial class Initializer_InitializeVehicles
    {
        public static void InitializeVehicles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle
                {
                    Id = new Guid("87073b93944f40b3b9c8bf9e9b1978a1"),
                    Manufacturer = "Volkswagen",
                    Model = "Passat",
                    BodyType = BodyType.Sedan,
                    FuelType = FuelType.Diesel,
                    ManufactureDate = DateTime.Now.AddYears(-7).AddDays(-43),
                    Seats = 5,
                    LicencePlate = "IS-28-XYZ",
                    PriceUsd = 20
                },
                new Vehicle
                {
                    Id = new Guid("dc4c684548104046980b19236f3a5135"),
                    Manufacturer = "Mercedes-Benz",
                    Model = "Vito",
                    BodyType = BodyType.Minivan,
                    FuelType = FuelType.Diesel,
                    ManufactureDate = DateTime.Now.AddYears(-10).AddDays(-3),
                    Seats = 5,
                    LicencePlate = "BT-14-TKO",
                    PriceUsd = 25
                },
                new Vehicle
                {
                    Id = new Guid("b09e0edd2d7f4b1eb2c819600b349164"),
                    Manufacturer = "Tesla",
                    Model = "Semi",
                    BodyType = BodyType.Truck,
                    FuelType = FuelType.Electric,
                    ManufactureDate = DateTime.Now.AddYears(-1).AddDays(-4),
                    Seats = 5,
                    LicencePlate = "SV-02-AGZ",
                    PriceUsd = 40
                }
            );
        }
    }
}
