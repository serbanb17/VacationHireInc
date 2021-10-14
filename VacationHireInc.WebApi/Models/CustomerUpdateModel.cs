// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.WebApi.Models
{
    public class CustomerUpdateModel
    {
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

        public Customer SetCustomer(Customer customer)
        {
            customer.Id = Id;
            customer.Name = Name;
            customer.Surname = Surname;
            customer.Email = Email;
            customer.PhoneNumber = PhoneNumber;
            customer.Birthday = Birthday;
            customer.Country = Country;
            customer.County = County;
            customer.City = City;
            customer.ZipCode = ZipCode;
            customer.Address = Address;
            return customer;
        }
    }
}
