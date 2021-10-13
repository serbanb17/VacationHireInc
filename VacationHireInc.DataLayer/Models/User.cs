// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System.Collections.Generic;

namespace VacationHireInc.DataLayer.Models
{
    public enum Privilege : byte { Admin, Clerk };
    public class User : BaseModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Privilege Privilege { get; set; }
        public List<VehicleOrder> VehicleOrders { get; set; }
    }
}
