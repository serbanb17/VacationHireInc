// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.WebApi.Models
{
    public class UserCreateModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Privilege Privilege { get; set; }

        public User GetUser() => new User
        {
            UserName = UserName,
            Name = Name,
            Surname = Surname,
            Email = Email,
            Password = Password,
            Privilege = Privilege
        };
    }
}
