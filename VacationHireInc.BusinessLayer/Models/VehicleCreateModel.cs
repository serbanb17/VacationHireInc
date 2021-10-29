// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.BusinessLayer.Models
{
    public class VehicleCreateModel
    {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public BodyType BodyType { get; set; }
        public FuelType FuelType { get; set; }
        public DateTime ManufactureDate { get; set; }
        public byte Seats { get; set; }
        public string LicencePlate { get; set; }
        public decimal PriceUsd { get; set; }

        public Vehicle GetVehicle() => new Vehicle
        {
            Manufacturer = Manufacturer,
            Model = Model,
            BodyType = BodyType,
            FuelType = FuelType,
            ManufactureDate = ManufactureDate,
            Seats = Seats,
            LicencePlate = LicencePlate,
            PriceUsd = PriceUsd
        };
    }
}
