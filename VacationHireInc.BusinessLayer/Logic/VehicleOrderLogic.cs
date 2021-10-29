// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using System.Collections.Generic;
using System.Linq;
using VacationHireInc.BusinessLayer.Interfaces;
using VacationHireInc.BusinessLayer.Models;
using VacationHireInc.DataLayer.Interfaces;
using VacationHireInc.DataLayer.Models;
using VacationHireInc.Security.Interfaces;

namespace VacationHireInc.BusinessLayer.Logic
{
    public class VehicleOrderLogic : IVehicleOrderLogic
    {
        private readonly IDataAccessProvider _dataAccessProvider;
        private readonly IJwtHelper _jwtHelper;

        public VehicleOrderLogic(IDataAccessProvider dataAccessProvider, IJwtHelper jwtHelper)
        {
            _dataAccessProvider = dataAccessProvider;
            _jwtHelper = jwtHelper;
        }

        public LogicResult<VehicleOrderGetModel> Get(string token, Guid id, Dictionary<string, decimal> usdRates)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out _))
                return new LogicResult<VehicleOrderGetModel>(ResultCode.Unauthorized);

            VehicleOrder vehicleOrder = _dataAccessProvider.VehicleOrderRepository.Get(id);
            if (vehicleOrder is null)
                return new LogicResult<VehicleOrderGetModel>(ResultCode.NotFound);

            var vehicleOrderGetModel = new VehicleOrderGetModel(vehicleOrder, usdRates);

            return new LogicResult<VehicleOrderGetModel>(vehicleOrderGetModel, ResultCode.Ok);
        }

        public LogicResult<int> GetCount(string token)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out _))
                return new LogicResult<int>(ResultCode.Unauthorized);

            int vehicleOrderCount = _dataAccessProvider.VehicleOrderRepository.GetCount();
            return new LogicResult<int>(vehicleOrderCount, ResultCode.Ok);
        }

        public LogicResult<List<VehicleOrderGetModel>> GetPage(string token, int pageId, int pageSize, Dictionary<string, decimal> usdRates)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out _))
                return new LogicResult<List<VehicleOrderGetModel>>(ResultCode.Unauthorized);

            List<VehicleOrder> vehicleOrdersPage = _dataAccessProvider.VehicleOrderRepository.GetPage(pageId, pageSize).ToList();
            var vehicleOrderGetModels = vehicleOrdersPage.Select(vo => new VehicleOrderGetModel(vo, usdRates)).ToList();
            return new LogicResult<List<VehicleOrderGetModel>>(vehicleOrderGetModels, ResultCode.Ok);
        }

        public LogicResult<List<VehicleOrderGetModel>> GetByFilter(string token, VehicleOrderFilter vehicleOrderFilter, Dictionary<string, decimal> usdRates)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out _))
                return new LogicResult<List<VehicleOrderGetModel>>(ResultCode.Unauthorized);

            List<VehicleOrder> vehicleOrders = _dataAccessProvider.VehicleOrderRepository.GetByCondition(c =>
                (vehicleOrderFilter.Status == null || vehicleOrderFilter.Status == c.Status)
                && (vehicleOrderFilter.CustomerId == null || vehicleOrderFilter.CustomerId == c.CustomerId)
                && (vehicleOrderFilter.UserId == null || vehicleOrderFilter.UserId == c.UserId)
                && (vehicleOrderFilter.VehicleId == null || vehicleOrderFilter.VehicleId == c.VehicleId)
                && (vehicleOrderFilter.OrderDateMin == null || vehicleOrderFilter.OrderDateMin <= c.OrderDate)
                && (vehicleOrderFilter.OrderDateMax == null || vehicleOrderFilter.OrderDateMax >= c.OrderDate)
                && (vehicleOrderFilter.ExpectedReturnDateMin == null || vehicleOrderFilter.ExpectedReturnDateMin <= c.ExpectedReturnDate)
                && (vehicleOrderFilter.ExpectedReturnDateMax == null || vehicleOrderFilter.ExpectedReturnDateMax >= c.ExpectedReturnDate)
                && (vehicleOrderFilter.ActualReturnDateMin == null || vehicleOrderFilter.ActualReturnDateMin <= c.ActualReturnDate)
                && (vehicleOrderFilter.ActualReturnDateMax == null || vehicleOrderFilter.ActualReturnDateMax >= c.ActualReturnDate)
            ).ToList();

            var vehicleOrderGetModels = vehicleOrders.Select(vo => new VehicleOrderGetModel(vo, usdRates)).ToList();

            return new LogicResult<List<VehicleOrderGetModel>>(vehicleOrderGetModels, ResultCode.Ok);
        }

        public LogicResult<object> Create(string token, VehicleOrderCreateModel vehicleOrder)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return new LogicResult<object>(ResultCode.Unauthorized);

            Customer customer = _dataAccessProvider.CustomerRepository.Get(vehicleOrder.CustomerId);
            if (customer is null)
                return new LogicResult<object>(ResultCode.NotFound, "Customer not found!");

            Vehicle vehicle = _dataAccessProvider.VehicleRepository.Get(vehicleOrder.VehicleId);
            if (vehicle is null)
                return new LogicResult<object>(ResultCode.NotFound, "Vehicle not found!");

            _dataAccessProvider.VehicleOrderRepository.Create(vehicleOrder.GetVehicleOrder(userId));
            _dataAccessProvider.Save();

            return new LogicResult<object>(null, ResultCode.Created, string.Empty);
        }

        public LogicResult<Guid> Update(string token, VehicleOrderUpdateModel updatedVehicleOrder)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return new LogicResult<Guid>(ResultCode.Unauthorized);

            Customer customer = _dataAccessProvider.CustomerRepository.Get(updatedVehicleOrder.CustomerId);
            if (customer is null)
                return new LogicResult<Guid>(ResultCode.NotFound, "Customer not found!");

            Vehicle vehicle = _dataAccessProvider.VehicleRepository.Get(updatedVehicleOrder.VehicleId);
            if (vehicle is null)
                return new LogicResult<Guid>(ResultCode.NotFound, "Vehicle not found!");

            VehicleOrder vehicleOrderToUpdate = _dataAccessProvider.VehicleOrderRepository.Get(updatedVehicleOrder.Id);
            if (vehicleOrderToUpdate is null)
                return new LogicResult<Guid>(ResultCode.NotFound, "Vehicle order not found!");

            if (userId != updatedVehicleOrder.UserId)
            {
                User user = _dataAccessProvider.UserRepository.Get(updatedVehicleOrder.UserId);
                if (user is null)
                    return new LogicResult<Guid>(ResultCode.NotFound, "User not found not found!");
            }

            if (vehicleOrderToUpdate.UserId != userId)
                return new LogicResult<Guid>(ResultCode.Unauthorized, "Only user assigned to an order can update that order!");

            _dataAccessProvider.VehicleOrderRepository.Update(updatedVehicleOrder.SetVehicleOrder(vehicleOrderToUpdate));
            _dataAccessProvider.Save();

            return new LogicResult<Guid>(vehicleOrderToUpdate.Id, ResultCode.Ok);
        }

        public LogicResult<Guid> Delete(string token, Guid id)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return new LogicResult<Guid>(ResultCode.Unauthorized);

            VehicleOrder vehicleOrder = _dataAccessProvider.VehicleOrderRepository.Get(id);
            if (vehicleOrder is null)
                return new LogicResult<Guid>(ResultCode.NotFound);

            if (vehicleOrder.UserId != userId)
                return new LogicResult<Guid>(ResultCode.Unauthorized, "Only user assigned to an order can delete that order!");

            _dataAccessProvider.VehicleOrderRepository.Delete(new VehicleOrder { Id = id });
            _dataAccessProvider.Save();

            return new LogicResult<Guid>(id, ResultCode.Ok);
        }
    }
}
