// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using VacationHireInc.DataLayer.Interfaces;
using VacationHireInc.DataLayer.Models;
using VacationHireInc.Security.Interfaces;
using VacationHireInc.WebApi.Interfaces;
using VacationHireInc.WebApi.Models;

namespace VacationHireInc.WebApi.Controllers
{
    [Route("api/v1/vehicle")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IDataAccessProvider _dataAccessProvider;
        private readonly IJwtHelper _jwtHelper;
        private readonly ICurrencyRatesUsdProvider _currencyRatesUsdProvider;

        public VehicleController(IDataAccessProvider dataAccessProvider, IJwtHelper jwtHelper, ICurrencyRatesUsdProvider currencyRatesUsdProvider)
        {
            _dataAccessProvider = dataAccessProvider;
            _jwtHelper = jwtHelper;
            _currencyRatesUsdProvider = currencyRatesUsdProvider;
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] Guid id)
        {
            var usdRatesTask = _currencyRatesUsdProvider.GetRatesAsync();
            Vehicle vehicle = _dataAccessProvider.VehicleRepository.Get(id);
            if (vehicle is null)
                return NotFound();

            var vehicleGetModel = new VehicleGetModel(vehicle, usdRatesTask.Result);

            return Ok(vehicleGetModel);
        }

        [HttpGet("fuelTypes")]
        public IActionResult GetFuelTypes()
        {
            List<string[]> fuelTypes = Enum.GetValues(typeof(FuelType)).Cast<FuelType>().Select(f => new[] { ((int)f).ToString(), f.ToString() }).ToList();
            return Ok(fuelTypes);
        }

        [HttpGet("bodyTypes")]
        public IActionResult GetBodyTypes()
        {
            List<string[]> bodyTypes = Enum.GetValues(typeof(BodyType)).Cast<BodyType>().Select(b => new[] { ((int)b).ToString(), b.ToString() }).ToList();
            return Ok(bodyTypes);
        }

        [HttpGet("count")]
        public IActionResult GetCount()
        {
            int vehicleCount = _dataAccessProvider.VehicleRepository.GetCount();
            return Ok(vehicleCount);
        }

        [HttpGet("page/{pageId}/{pageSize}")]
        public IActionResult GetPage([FromRoute] int pageId, [FromRoute] int pageSize)
        {
            var usdRatesTask = _currencyRatesUsdProvider.GetRatesAsync();
            List<Vehicle> vehiclesPage = _dataAccessProvider.VehicleRepository.GetPage(pageId, pageSize).ToList();
            var vehicleGetModels = vehiclesPage.Select(v => new VehicleGetModel(v, usdRatesTask.Result)).ToList();
            return Ok(vehicleGetModels);
        }

        [HttpPost("filter")]
        public IActionResult GetByFilter([FromHeader(Name = "Authorization")] string token, [FromBody] VehicleFilter vehicleFilter)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out _))
                return Unauthorized();

            var usdRatesTask = _currencyRatesUsdProvider.GetRatesAsync();

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

            var vehicleGetModels = vehicles.Select(v => new VehicleGetModel(v, usdRatesTask.Result)).ToList();

            return Ok(vehicleGetModels);
        }

        [HttpPost]
        public IActionResult Create([FromHeader(Name = "Authorization")] string token, [FromBody] VehicleCreateModel newVehicle)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return Unauthorized();

            Vehicle duplicateVehicle = _dataAccessProvider.VehicleRepository.GetByCondition(c => c.LicencePlate == newVehicle.LicencePlate).FirstOrDefault();
            if (duplicateVehicle != null)
                return BadRequest("A vehicle with same licence plate exists!");

            _dataAccessProvider.VehicleRepository.Create(newVehicle.GetVehicle());
            _dataAccessProvider.Save();

            return Created("", null);
        }

        [HttpPut]
        public IActionResult Update([FromHeader(Name = "Authorization")] string token, [FromBody] VehicleUpdateModel updatedVehicle)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return Unauthorized();
            
            Vehicle duplicateVehicle = _dataAccessProvider.VehicleRepository.GetByCondition(c => 
                c.Id != updatedVehicle.Id && c.LicencePlate == updatedVehicle.LicencePlate
            ).FirstOrDefault();
            if (duplicateVehicle != null)
                return BadRequest("A vehicle with same licence plate exists!");

            Vehicle vehicleToUpdate = _dataAccessProvider.VehicleRepository.Get(updatedVehicle.Id);
            if (vehicleToUpdate == null)
                return BadRequest("Could not find vehicle to update!");

            _dataAccessProvider.VehicleRepository.Update(updatedVehicle.SetVehicle(vehicleToUpdate));
            _dataAccessProvider.Save();

            return Ok(vehicleToUpdate.Id);
        }

        [HttpDelete]
        public IActionResult Delete([FromHeader(Name = "Authorization")] string token, [FromBody] Guid id)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out _))
                return Unauthorized();

            Vehicle vehicleToUpdate = _dataAccessProvider.VehicleRepository.Get(id);
            if (vehicleToUpdate == null)
                return BadRequest("Could not find vehicle to delete!");

            _dataAccessProvider.VehicleRepository.Delete(new Vehicle { Id = id });
            _dataAccessProvider.Save();

            return Ok(id);
        }
    }
}
