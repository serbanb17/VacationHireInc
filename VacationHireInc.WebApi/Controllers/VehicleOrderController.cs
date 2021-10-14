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
    [Route("api/v1/vehicleorder")]
    [ApiController]
    public class VehicleOrderController : ControllerBase
    {
        private readonly IDataAccessProvider _dataAccessProvider;
        private readonly IJwtHelper _jwtHelper;
        private readonly ICurrencyRatesUsdProvider _currencyRatesUsdProvider;

        public VehicleOrderController(IDataAccessProvider dataAccessProvider, IJwtHelper jwtHelper, ICurrencyRatesUsdProvider currencyRatesUsdProvider)
        {
            _dataAccessProvider = dataAccessProvider;
            _jwtHelper = jwtHelper;
            _currencyRatesUsdProvider = currencyRatesUsdProvider;
        }

        [HttpPost("{id}")]
        public IActionResult Get([FromHeader(Name = "Authorization")] string token, [FromRoute] Guid id)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out _))
                return Unauthorized();

            var usdRatesTask = _currencyRatesUsdProvider.GetRatesAsync();

            VehicleOrder vehicleOrder = _dataAccessProvider.VehicleOrderRepository.Get(id);
            if (vehicleOrder is null)
                return NotFound();

            var vehicleOrderGetModel = new VehicleOrderGetModel(vehicleOrder, usdRatesTask.Result);

            return Ok(vehicleOrderGetModel);
        }

        [HttpGet("count")]
        public IActionResult GetCount([FromHeader(Name = "Authorization")] string token)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out _))
                return Unauthorized();

            int vehicleOrderCount = _dataAccessProvider.VehicleOrderRepository.GetCount();
            return Ok(vehicleOrderCount);
        }

        [HttpGet("page/{pageId}/{pageSize}")]
        public IActionResult GetPage([FromHeader(Name = "Authorization")] string token, [FromRoute] int pageId, [FromRoute] int pageSize)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out _))
                return Unauthorized();

            var usdRatesTask = _currencyRatesUsdProvider.GetRatesAsync();
            List<VehicleOrder> vehicleOrdersPage = _dataAccessProvider.VehicleOrderRepository.GetPage(pageId, pageSize).ToList();
            var vehicleOrderGetModels = vehicleOrdersPage.Select(vo => new VehicleOrderGetModel(vo, usdRatesTask.Result));
            return Ok(vehicleOrderGetModels);
        }

        [HttpPost("filter")]
        public IActionResult GetByFilter([FromHeader(Name = "Authorization")] string token, [FromBody] VehicleOrderFilter vehicleOrderFilter)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out _))
                return Unauthorized();

            var usdRatesTask = _currencyRatesUsdProvider.GetRatesAsync();

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

            var vehicleOrderGetModels = vehicleOrders.Select(vo => new VehicleOrderGetModel(vo, usdRatesTask.Result));

            return Ok(vehicleOrderGetModels);
        }

        [HttpPost]
        public IActionResult Create([FromHeader(Name = "Authorization")] string token, [FromBody] VehicleOrderCreateModel vehicleOrder)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return Unauthorized();

            Customer customer = _dataAccessProvider.CustomerRepository.Get(vehicleOrder.CustomerId);
            if (customer is null)
                return NotFound("Customer not found!");

            Vehicle vehicle = _dataAccessProvider.VehicleRepository.Get(vehicleOrder.VehicleId);
            if (vehicle is null)
                return NotFound("Vehicle not found!");

            _dataAccessProvider.VehicleOrderRepository.Create(vehicleOrder.GetVehicleOrder(userId));
            _dataAccessProvider.Save();
            
            return Created("", null);
        }

        [HttpPut]
        public IActionResult Update([FromHeader(Name = "Authorization")] string token, [FromBody] VehicleOrderUpdateModel updatedVehicleOrder)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return Unauthorized();

            Customer customer = _dataAccessProvider.CustomerRepository.Get(updatedVehicleOrder.CustomerId);
            if (customer is null)
                return NotFound("Customer not found!");

            Vehicle vehicle = _dataAccessProvider.VehicleRepository.Get(updatedVehicleOrder.VehicleId);
            if (vehicle is null)
                return NotFound("Vehicle not found!");

            VehicleOrder vehicleOrderToUpdate = _dataAccessProvider.VehicleOrderRepository.Get(updatedVehicleOrder.Id);
            if (vehicleOrderToUpdate is null)
                return NotFound("Vehicle order not found!");

            if (userId != updatedVehicleOrder.UserId)
            {
                User user = _dataAccessProvider.UserRepository.Get(updatedVehicleOrder.UserId);
                if (user is null)
                    return NotFound("User not found not found!");
            }

            if (vehicleOrderToUpdate.UserId != userId)
                return Unauthorized("Only user assigned to an order can update that order!");

            _dataAccessProvider.VehicleOrderRepository.Update(updatedVehicleOrder.SetVehicleOrder(vehicleOrderToUpdate));
            _dataAccessProvider.Save();

            return Ok(vehicleOrderToUpdate.Id);
        }

        [HttpDelete]
        public IActionResult Delete([FromHeader(Name = "Authorization")] string token, [FromBody] Guid id)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return Unauthorized();

            VehicleOrder vehicleOrder = _dataAccessProvider.VehicleOrderRepository.Get(id);
            if (vehicleOrder is null)
                return NotFound();

            if (vehicleOrder.UserId != userId)
                return Unauthorized("Only user assigned to an order can delete that order!");

            _dataAccessProvider.VehicleOrderRepository.Delete(new VehicleOrder { Id = id });
            _dataAccessProvider.Save();

            return Ok(id);
        }
    }
}
