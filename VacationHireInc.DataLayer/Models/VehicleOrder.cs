// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;

namespace VacationHireInc.DataLayer.Models
{
    public enum Status : byte { Hired, Returned }
    public class VehicleOrder : BaseModel
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

        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
