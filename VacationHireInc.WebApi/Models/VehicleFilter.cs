// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.WebApi.Models
{
    public class VehicleFilter
    {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public BodyType? BodyType { get; set; }
        public FuelType? FuelType { get; set; }
        public DateTime? ManufactureDateMin { get; set; }
        public DateTime? ManufactureDateMax { get; set; }
        public byte? Seats { get; set; }
        public string LicencePlate { get; set; }
        public decimal? PriceUsdMin { get; set; }
        public decimal? PriceUsdMax { get; set; }
    }
}
