// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using System.Collections.Generic;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.WebApi.Models
{
    public class VehicleOrderGetModel
    {
        public VehicleOrderGetModel()
        {
        }

        public VehicleOrderGetModel(VehicleOrder vehicleOrder) => SetProperties(vehicleOrder);

        public VehicleOrderGetModel(VehicleOrder vehicleOrder, Dictionary<string, decimal> usdRates) => SetProperties(vehicleOrder, usdRates);

        public Guid Id { get; set; }
        public Status Status { get; set; }
        public DateTime OrderDate { get; set; }
        public float FuelPercentageOnOrderDate { get; set; }
        public string OrderDateComments { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public DateTime ActualReturnDate { get; set; }
        public float FuelPercentageOnReturnDate { get; set; }
        public string ReturnDateComments { get; set; }
        public decimal PriceToPayUsd { get; set; }
        public string CurrencyPayedWith { get; set; }
        public Guid UserId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid VehicleId { get; set; }
        public Dictionary<string, decimal> OtherCurrencyPrice { get; set; }

        private void SetProperties(VehicleOrder vehicleOrder)
        {
            Id = vehicleOrder.Id;
            Status = vehicleOrder.Status;
            OrderDate = vehicleOrder.OrderDate;
            FuelPercentageOnOrderDate = vehicleOrder.FuelPercentageOnOrderDate;
            OrderDateComments = vehicleOrder.OrderDateComments;
            ExpectedReturnDate = vehicleOrder.ExpectedReturnDate;
            ActualReturnDate = vehicleOrder.ActualReturnDate;
            FuelPercentageOnReturnDate = vehicleOrder.FuelPercentageOnReturnDate;
            ReturnDateComments = vehicleOrder.ReturnDateComments;
            PriceToPayUsd = vehicleOrder.PriceToPayUsd;
            CurrencyPayedWith = vehicleOrder.CurrencyPayedWith;
            UserId = vehicleOrder.UserId;
            CustomerId = vehicleOrder.CustomerId;
            VehicleId = vehicleOrder.VehicleId;
        }

        private void SetProperties(VehicleOrder vehicleOrder, Dictionary<string, decimal> usdRates)
        {
            SetProperties(vehicleOrder);
            UpdateOtherCurrencyPrice(usdRates);
        }

        private void UpdateOtherCurrencyPrice(Dictionary<string, decimal> usdRates)
        {
            OtherCurrencyPrice = new Dictionary<string, decimal>();
            if (usdRates != null)
                foreach (var kv in usdRates)
                    OtherCurrencyPrice.Add(kv.Key, PriceToPayUsd * kv.Value);
        }
    }
}
