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
    public class VehicleLogic : IVehicleLogic
    {

        private readonly IDataAccessProvider _dataAccessProvider;
        private readonly IJwtHelper _jwtHelper;

        public VehicleLogic(IDataAccessProvider dataAccessProvider, IJwtHelper jwtHelper)
        {
            _dataAccessProvider = dataAccessProvider;
            _jwtHelper = jwtHelper;
        }

        public LogicResult<VehicleGetModel> Get(Guid id, Dictionary<string, decimal> usdRates)
        {
            Vehicle vehicle = _dataAccessProvider.VehicleRepository.Get(id);
            if (vehicle is null)
                return new LogicResult<VehicleGetModel>(ResultCode.NotFound);

            var vehicleGetModel = new VehicleGetModel(vehicle, usdRates);

            return new LogicResult<VehicleGetModel>(vehicleGetModel, ResultCode.Ok);
        }

        public LogicResult<List<string[]>> GetFuelTypes()
        {
            List<string[]> fuelTypes = Enum.GetValues(typeof(FuelType)).Cast<FuelType>().Select(f => new[] { ((int)f).ToString(), f.ToString() }).ToList();
            return new LogicResult<List<string[]>>(fuelTypes, ResultCode.Ok);
        }

        public LogicResult<List<string[]>> GetBodyTypes()
        {
            List<string[]> bodyTypes = Enum.GetValues(typeof(BodyType)).Cast<BodyType>().Select(b => new[] { ((int)b).ToString(), b.ToString() }).ToList();
            return new LogicResult<List<string[]>>(bodyTypes, ResultCode.Ok);
        }

        public LogicResult<int> GetCount()
        {
            int vehicleCount = _dataAccessProvider.VehicleRepository.GetCount();
            return new LogicResult<int>(vehicleCount, ResultCode.Ok);
        }

        public LogicResult<List<VehicleGetModel>> GetPage(int pageId, int pageSize, Dictionary<string, decimal> usdRates)
        {
            List<Vehicle> vehiclesPage = _dataAccessProvider.VehicleRepository.GetPage(pageId, pageSize).ToList();
            var vehicleGetModels = vehiclesPage.Select(v => new VehicleGetModel(v, usdRates)).ToList();
            return new LogicResult<List<VehicleGetModel>>(vehicleGetModels, ResultCode.Ok);
        }

        public LogicResult<List<VehicleGetModel>> GetByFilter(string token, VehicleFilter vehicleFilter, Dictionary<string, decimal> usdRates)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out _))
                return new LogicResult<List<VehicleGetModel>>(ResultCode.Unauthorized);

            List<Vehicle> vehicles = _dataAccessProvider.VehicleRepository.GetByCondition(c =>
                (vehicleFilter.Manufacturer == null || vehicleFilter.Manufacturer.ToLower().Contains(c.Manufacturer.ToLower()))
                && (vehicleFilter.Model == null || vehicleFilter.Model.ToLower().Contains(c.Model.ToLower()))
                && (vehicleFilter.BodyType == null || vehicleFilter.BodyType == c.BodyType)
                && (vehicleFilter.FuelType == null || vehicleFilter.FuelType == c.FuelType)
                && (vehicleFilter.ManufactureDateMin == null || vehicleFilter.ManufactureDateMin <= c.ManufactureDate)
                && (vehicleFilter.ManufactureDateMax == null || vehicleFilter.ManufactureDateMax >= c.ManufactureDate)
                && (vehicleFilter.Seats == null || vehicleFilter.Seats == c.Seats)
                && (vehicleFilter.LicencePlate == null || vehicleFilter.LicencePlate == c.LicencePlate)
                && (vehicleFilter.PriceUsdMin == null || vehicleFilter.PriceUsdMin <= c.PriceUsd)
                && (vehicleFilter.PriceUsdMax == null || vehicleFilter.PriceUsdMax >= c.PriceUsd)
            ).ToList();

            var vehicleGetModels = vehicles.Select(v => new VehicleGetModel(v, usdRates)).ToList();

            return new LogicResult<List<VehicleGetModel>>(vehicleGetModels, ResultCode.Ok);
        }

        public LogicResult<object> Create(string token, VehicleCreateModel newVehicle)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return new LogicResult<object>(ResultCode.Unauthorized);

            Vehicle duplicateVehicle = _dataAccessProvider.VehicleRepository.GetByCondition(c => c.LicencePlate == newVehicle.LicencePlate).FirstOrDefault();
            if (duplicateVehicle != null)
                return new LogicResult<object>(ResultCode.BadRequest, "A vehicle with same licence plate exists!");

            _dataAccessProvider.VehicleRepository.Create(newVehicle.GetVehicle());
            _dataAccessProvider.Save();

            return new LogicResult<object>(null, ResultCode.Created, string.Empty);
        }

        public LogicResult<Guid> Update(string token, VehicleUpdateModel updatedVehicle)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return new LogicResult<Guid>(ResultCode.Unauthorized);

            Vehicle duplicateVehicle = _dataAccessProvider.VehicleRepository.GetByCondition(c =>
                c.Id != updatedVehicle.Id && c.LicencePlate == updatedVehicle.LicencePlate
            ).FirstOrDefault();
            if (duplicateVehicle != null)
                return new LogicResult<Guid>(ResultCode.BadRequest, "A vehicle with same licence plate exists!");

            Vehicle vehicleToUpdate = _dataAccessProvider.VehicleRepository.Get(updatedVehicle.Id);
            if (vehicleToUpdate == null)
                return new LogicResult<Guid>(ResultCode.BadRequest, "Could not find vehicle to update!");

            _dataAccessProvider.VehicleRepository.Update(updatedVehicle.SetVehicle(vehicleToUpdate));
            _dataAccessProvider.Save();

            return new LogicResult<Guid>(vehicleToUpdate.Id, ResultCode.Ok);
        }

        public LogicResult<Guid> Delete(string token, Guid id)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out _))
                return new LogicResult<Guid>(ResultCode.Unauthorized);

            Vehicle vehicleToUpdate = _dataAccessProvider.VehicleRepository.Get(id);
            if (vehicleToUpdate == null)
                return new LogicResult<Guid>(ResultCode.BadRequest, "Could not find vehicle to delete!");

            _dataAccessProvider.VehicleRepository.Delete(new Vehicle { Id = id });
            _dataAccessProvider.Save();

            return new LogicResult<Guid>(id, ResultCode.Ok);
        }
    }
}
