﻿// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.BusinessLayer.Models
{
    public class VehicleUpdateModel
    {
        public Guid Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public BodyType BodyType { get; set; }
        public FuelType FuelType { get; set; }
        public DateTime ManufactureDate { get; set; }
        public byte Seats { get; set; }
        public string LicencePlate { get; set; }
        public decimal PriceUsd { get; set; }

        public Vehicle SetVehicle(Vehicle vehicle)
        {
            vehicle.Id = Id;
            vehicle.Manufacturer = Manufacturer;
            vehicle.Model = Model;
            vehicle.BodyType = BodyType;
            vehicle.FuelType = FuelType;
            vehicle.ManufactureDate = ManufactureDate;
            vehicle.Seats = Seats;
            vehicle.LicencePlate = LicencePlate;
            vehicle.PriceUsd = PriceUsd;
            return vehicle;
        }
    }
}
