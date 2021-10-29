// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.BusinessLayer.Models
{
    public class UserGetModel
    {
        public UserGetModel()
        {
        }

        public UserGetModel(User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Name = user.Name;
            Surname = user.Surname;
            Email = user.Email;
            Privilege = user.Privilege;
        }

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public Privilege Privilege { get; set; }
    }
}
