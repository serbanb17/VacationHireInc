// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using System.Collections.Generic;
using VacationHireInc.BusinessLayer.Models;

namespace VacationHireInc.BusinessLayer.Interfaces
{
    public interface IVehicleLogic
    {
        public LogicResult<VehicleGetModel> Get(Guid id, Dictionary<string, decimal> usdRates);

        public LogicResult<List<string[]>> GetFuelTypes();

        public LogicResult<List<string[]>> GetBodyTypes();

        public LogicResult<int> GetCount();

        public LogicResult<List<VehicleGetModel>> GetPage(int pageId, int pageSize, Dictionary<string, decimal> usdRates);

        public LogicResult<List<VehicleGetModel>> GetByFilter(string token, VehicleFilter vehicleFilter, Dictionary<string, decimal> usdRates);

        public LogicResult<object> Create(string token, VehicleCreateModel newVehicle);

        public LogicResult<Guid> Update(string token, VehicleUpdateModel updatedVehicle);

        public LogicResult<Guid> Delete(string token, Guid id);
    }
}
