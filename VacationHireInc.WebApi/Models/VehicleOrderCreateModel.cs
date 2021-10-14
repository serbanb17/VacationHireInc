// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.WebApi.Models
{
    public class VehicleOrderCreateModel
    {
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
        public Guid CustomerId { get; set; }
        public Guid VehicleId { get; set; }

        public VehicleOrder GetVehicleOrder(Guid userId) => new VehicleOrder
        {
            Status = Status,
            OrderDate = OrderDate,
            FuelPercentageOnOrderDate = FuelPercentageOnOrderDate,
            OrderDateComments = OrderDateComments,
            ExpectedReturnDate = ExpectedReturnDate,
            ActualReturnDate = ActualReturnDate,
            FuelPercentageOnReturnDate = FuelPercentageOnReturnDate,
            ReturnDateComments = ReturnDateComments,
            PriceToPayUsd = PriceToPayUsd,
            CurrencyPayedWith = CurrencyPayedWith,
            UserId = userId,
            CustomerId = CustomerId,
            VehicleId = VehicleId
        };
    }
}
