// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.EntityFrameworkCore;
using System;
using VacationHireInc.DataLayer.Models;
using VacationHireInc.Security;
using VacationHireInc.Security.Interfaces;

namespace VacationHireInc.DataLayer.Initializers
{
    internal static class Initializer
    {
        private static IHashingHelper _hashingHelper = new HashingHelper(new byte[0]);
        
        public static void SetPasswordSalt(byte[] salt) => _hashingHelper = new HashingHelper(salt);

        public static void InitializeUsers(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = new Guid("43fcebc353934991ba77f4e4a33683f5"),
                    Name = "John",
                    Surname = "Doe",
                    UserName = "johndoe",
                    Email = "johndoe@vacationhireinc.com",
                    Password = _hashingHelper.SaltHash("johndoepw"),
                    Privilege = Privilege.Admin
                },
                new User
                {
                    Id = new Guid("6312cc5e9d26450098aa38fa7484775f"),
                    Name = "Kenneth",
                    Surname = "Alister",
                    UserName = "kennethalister",
                    Email = "kennethalister@vacationhireinc.com",
                    Password = _hashingHelper.SaltHash("kennethalisterpw"),
                    Privilege = Privilege.Admin
                },
                new User
                {
                    Id = new Guid("2ef6d12ae5434892ad5d91fa5c79d378"),
                    Name = "Mtendere",
                    Surname = "Timmy",
                    UserName = "mtenderetimmy",
                    Email = "mtenderetimmy@vacationhireinc.com",
                    Password = _hashingHelper.SaltHash("mtenderetimmypw"),
                    Privilege = Privilege.Clerk
                },
                new User
                {
                    Id = new Guid("c2f32c6d56d04f31b6f30934b705348e"),
                    Name = "Trang",
                    Surname = "Devorah",
                    UserName = "trangdevorah",
                    Email = "trangdevorah@vacationhireinc.com",
                    Password = _hashingHelper.SaltHash("trangdevorahpw"),
                    Privilege = Privilege.Clerk
                },
                new User
                {
                    Id = new Guid("5869e8888b6040ca936c47a4af0922e0"),
                    Name = "Marianna",
                    Surname = "Ketil",
                    UserName = "mariannaketil",
                    Email = "mariannaketil@vacationhireinc.com",
                    Password = _hashingHelper.SaltHash("mariannaketilpw"),
                    Privilege = Privilege.Clerk
                }
            );
        }
    }
}
