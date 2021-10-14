// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.WebApi.Models
{
    public class CustomerGetModel
    {
        public CustomerGetModel()
        {
        }

        public CustomerGetModel(Customer customer)
        {
            Id = customer.Id;
            Name = customer.Name;
            Surname = customer.Surname;
            Email = customer.Email;
            PhoneNumber = customer.PhoneNumber;
            Birthday = customer.Birthday;
            Country = customer.Country;
            County = customer.County;
            City = customer.City;
            ZipCode = customer.ZipCode;
            Address = customer.Address;
        }

        public Guid Id { get; set; }
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
    }
}
