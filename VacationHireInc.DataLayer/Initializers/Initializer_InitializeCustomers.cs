// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.EntityFrameworkCore;
using System;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.DataLayer.Initializers
{
    internal static partial class Initializer_InitializeCustomers
    {
        public static void InitializeCustomers(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = new Guid("19793cea8e4a46f88f1188746ff1f632"),
                    Name = "Melinda",
                    Surname = "Duffey",
                    Email = "melindaduffey@gmail.com",
                    PhoneNumber = "+40727705604",
                    Birthday = DateTime.Now.AddYears(-23)
                },
                new Customer
                {
                    Id = new Guid("68613432a9cb4d818eac0f326244c002"),
                    Name = "Amir",
                    Surname = "Signý",
                    Email = "amirsigný@gmail.com",
                    PhoneNumber = "+14109938731",
                    Birthday = DateTime.Now.AddYears(-54)
                },
                new Customer
                {
                    Id = new Guid("05758d12f4cb4a74af18dceed1b9dc5c"),
                    Name = "Charmion",
                    Surname = "Ealhhere",
                    Email = "charmionealhhere@gmail.com",
                    PhoneNumber = "+31624006475",
                    Birthday = DateTime.Now.AddYears(-38)
                }
            );
        }
    }
}
