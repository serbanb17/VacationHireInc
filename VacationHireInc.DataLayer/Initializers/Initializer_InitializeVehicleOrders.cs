// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.EntityFrameworkCore;
using System;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.DataLayer.Initializers
{
    internal static partial class Initializer_InitializeVehicleOrders
    {
        public static void InitializeVehicleOrders(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleOrder>().HasData(
                new VehicleOrder
                {
                    Id = Guid.NewGuid(),
                    Status = Status.Returned,
                    OrderDate = DateTime.Now.AddDays(-10),
                    FuelPercentageOnOrderDate = 100f,
                    OrderDateComments = "",
                    ExpectedReturnDate = DateTime.Now.AddDays(-7),
                    ActualReturnDate = DateTime.Now.AddDays(-7),
                    FuelPercentageOnReturnDate = 25.5f,
                    ReturnDateComments = "",
                    PriceToPayUsd = 180.2M,
                    UserId = new Guid("2ef6d12ae5434892ad5d91fa5c79d378"),
                    CustomerId = new Guid("19793cea8e4a46f88f1188746ff1f632"),
                    VehicleId = new Guid("87073b93944f40b3b9c8bf9e9b1978a1"),
                },
                new VehicleOrder
                {
                    Id = Guid.NewGuid(),
                    Status = Status.Returned,
                    OrderDate = DateTime.Now.AddDays(-20),
                    FuelPercentageOnOrderDate = 90f,
                    OrderDateComments = "",
                    ExpectedReturnDate = DateTime.Now.AddDays(-10),
                    ActualReturnDate = DateTime.Now.AddDays(-5),
                    FuelPercentageOnReturnDate = 80.3f,
                    ReturnDateComments = "Late on return",
                    PriceToPayUsd = 478.2M,
                    UserId = new Guid("c2f32c6d56d04f31b6f30934b705348e"),
                    CustomerId = new Guid("68613432a9cb4d818eac0f326244c002"),
                    VehicleId = new Guid("dc4c684548104046980b19236f3a5135"),
                },
                new VehicleOrder
                {
                    Id = Guid.NewGuid(),
                    Status = Status.Returned,
                    OrderDate = DateTime.Now.AddDays(-40),
                    FuelPercentageOnOrderDate = 30f,
                    OrderDateComments = "",
                    ExpectedReturnDate = DateTime.Now.AddDays(-15),
                    ActualReturnDate = DateTime.Now.AddDays(-17),
                    FuelPercentageOnReturnDate = 75f,
                    ReturnDateComments = "Early return",
                    PriceToPayUsd = 880.0M,
                    UserId = new Guid("5869e8888b6040ca936c47a4af0922e0"),
                    CustomerId = new Guid("05758d12f4cb4a74af18dceed1b9dc5c"),
                    VehicleId = new Guid("b09e0edd2d7f4b1eb2c819600b349164"),
                }
            );
        }
    }
}
