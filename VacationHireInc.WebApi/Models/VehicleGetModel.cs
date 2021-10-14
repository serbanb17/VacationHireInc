// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using System.Collections.Generic;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.WebApi.Models
{
    public class VehicleGetModel
    {
        public VehicleGetModel()
        {
        }

        public VehicleGetModel(Vehicle vehicle) => SetProperties(vehicle);

        public VehicleGetModel(Vehicle vehicle, Dictionary<string, decimal> usdRates) => SetProperties(vehicle, usdRates);


        public Guid Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public BodyType BodyType { get; set; }
        public FuelType FuelType { get; set; }
        public DateTime ManufactureDate { get; set; }
        public byte Seats { get; set; }
        public string LicencePlate { get; set; }
        public decimal PriceUsd { get; set; }
        public Dictionary<string, decimal> OtherCurrencyPrice { get; set; }

        private void SetProperties(Vehicle vehicle)
        {
            Id = vehicle.Id;
            Manufacturer = vehicle.Manufacturer;
            Model = vehicle.Model;
            BodyType = vehicle.BodyType;
            FuelType = vehicle.FuelType;
            ManufactureDate = vehicle.ManufactureDate;
            Seats = vehicle.Seats;
            LicencePlate = vehicle.LicencePlate;
            PriceUsd = vehicle.PriceUsd;
        }

        private void SetProperties(Vehicle vehicle, Dictionary<string, decimal> usdRates)
        {
            SetProperties(vehicle);
            UpdateOtherCurrencyPrice(usdRates);
        }

        private void UpdateOtherCurrencyPrice(Dictionary<string, decimal> usdRates)
        {
            OtherCurrencyPrice = new Dictionary<string, decimal>();
            if (usdRates != null)
                foreach (var kv in usdRates)
                    OtherCurrencyPrice.Add(kv.Key, PriceUsd * kv.Value);
        }
    }
}
