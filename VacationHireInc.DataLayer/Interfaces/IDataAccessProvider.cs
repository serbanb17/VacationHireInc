// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.DataLayer.Interfaces
{
    /// <summary>
    /// Implements Unit of Work.
    /// All changes applied using IRepository<T> properties are saved to database when Save() method is called.
    /// </summary>
    public interface IDataAccessProvider
    {
        IRepository<User> UserRepository { get; }
        IRepository<Vehicle> VehicleRepository { get; }
        IRepository<Customer> CustomerRepository { get; }
        IRepository<VehicleOrder> VehicleOrderRepository { get; }

        /// <summary>
        /// Saves applied repositories modifications to the database.
        /// </summary>
        void Save();
    }
}
