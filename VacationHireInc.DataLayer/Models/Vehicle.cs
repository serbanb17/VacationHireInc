// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using System.Collections.Generic;

namespace VacationHireInc.DataLayer.Models
{
    public enum BodyType : byte { Truck, Minivan, Sedan }
    public enum FuelType : byte { Diesel, CNG, LPG, Petrol, Electric, Hybrid }
    public class Vehicle : BaseModel
    {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public BodyType BodyType { get; set; }
        public FuelType FuelType { get; set; }
        public DateTime ManufactureDate { get; set; }
        public byte Seats { get; set; }
        public string LicencePlate { get; set; }
        public decimal PriceUsd { get; set; }
        public Dictionary<string, decimal> OtherCurrencyPrice { get; set; }
        public List<VehicleOrder> VehicleOrders { get; set; }
    }
}
