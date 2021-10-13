// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using VacationHireInc.DataLayer.Interfaces;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.DataLayer
{
    public class DataAccessProvider : IDataAccessProvider
    {
        private AppDbContext _appDbContext;

        public DataAccessProvider(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            UserRepository = new Repository<User>(appDbContext);
            VehicleRepository = new Repository<Vehicle>(appDbContext);
            CustomerRepository = new Repository<Customer>(appDbContext);
            VehicleOrderRepository = new Repository<VehicleOrder>(appDbContext);
        }

        public IRepository<User> UserRepository { get; }
        public IRepository<Vehicle> VehicleRepository { get; }
        public IRepository<Customer> CustomerRepository { get; }
        public IRepository<VehicleOrder> VehicleOrderRepository { get; }

        public void Save() => _appDbContext.SaveChanges();
    }
}
