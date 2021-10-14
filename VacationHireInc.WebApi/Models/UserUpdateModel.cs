// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.WebApi.Models
{
    public class UserUpdateModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Privilege Privilege { get; set; }

        public User SetUser(User user)
        {
            user.Id = Id;
            user.UserName = UserName;
            user.Name = Name;
            user.Surname = Surname;
            user.Email = Email;
            user.Password = Password;
            user.Privilege = Privilege;
            return user;
        }
    }
}
