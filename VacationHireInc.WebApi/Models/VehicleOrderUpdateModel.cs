// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.WebApi.Models
{
    public class VehicleOrderUpdateModel
    {
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

        public VehicleOrder SetVehicleOrder(VehicleOrder vehicleOrder)
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
            return vehicleOrder;
        }
    }
}
