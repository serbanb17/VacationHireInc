// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using System.Collections.Generic;
using VacationHireInc.BusinessLayer.Models;

namespace VacationHireInc.BusinessLayer.Interfaces
{
    public interface IVehicleOrderLogic
    {
        public LogicResult<VehicleOrderGetModel> Get(string token, Guid id, Dictionary<string, decimal> usdRates);

        public LogicResult<int> GetCount(string token);

        public LogicResult<List<VehicleOrderGetModel>> GetPage(string token, int pageId, int pageSize, Dictionary<string, decimal> usdRates);

        public LogicResult<List<VehicleOrderGetModel>> GetByFilter(string token, VehicleOrderFilter vehicleOrderFilter, Dictionary<string, decimal> usdRates);

        public LogicResult<object> Create(string token, VehicleOrderCreateModel vehicleOrder);

        public LogicResult<Guid> Update(string token, VehicleOrderUpdateModel updatedVehicleOrder);

        public LogicResult<Guid> Delete(string token, Guid id);
    }
}
