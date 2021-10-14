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
            vehicleOrder.Id = Id;
            vehicleOrder.Status = Status;
            vehicleOrder.OrderDate = OrderDate;
            vehicleOrder.FuelPercentageOnOrderDate = FuelPercentageOnOrderDate;
            vehicleOrder.OrderDateComments = OrderDateComments;
            vehicleOrder.ExpectedReturnDate = ExpectedReturnDate;
            vehicleOrder.ActualReturnDate = ActualReturnDate;
            vehicleOrder.FuelPercentageOnReturnDate = FuelPercentageOnReturnDate;
            vehicleOrder.ReturnDateComments = ReturnDateComments;
            vehicleOrder.PriceToPayUsd = PriceToPayUsd;
            vehicleOrder.CurrencyPayedWith = CurrencyPayedWith;
            vehicleOrder.UserId = UserId;
            vehicleOrder.CustomerId = CustomerId;
            vehicleOrder.VehicleId = VehicleId;
            return vehicleOrder;
        }
    }
}
