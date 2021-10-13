// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.WebApi.Models
{
    public class VehicleOrderFilter
    {
        public Status? Status { get; set; }
        public DateTime? OrderDateMin { get; set; }
        public DateTime? ExpectedReturnDateMin { get; set; }
        public DateTime? ActualReturnDateMin { get; set; }
        public DateTime? OrderDateMax { get; set; }
        public DateTime? ExpectedReturnDateMax { get; set; }
        public DateTime? ActualReturnDateMax { get; set; }
        public Guid? UserId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? VehicleId { get; set; }
    }
}
