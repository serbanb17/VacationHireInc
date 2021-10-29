// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.BusinessLayer.Models
{
    public class CustomerCreateModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }

        public Customer GetCustomer() => new Customer
        {
            Name = Name,
            Surname = Surname,
            Email = Email,
            PhoneNumber = PhoneNumber,
            Birthday = Birthday,
            Country = Country,
            County = County,
            City = City,
            ZipCode = ZipCode,
            Address = Address
        };
    }
}
