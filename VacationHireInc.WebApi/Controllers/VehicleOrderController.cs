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

            UpdateOtherCurrencyPrice(vehicleOrder, usdRatesTask.Result);

            return Ok(vehicleOrder);
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
            vehicleOrdersPage.ForEach(vo => UpdateOtherCurrencyPrice(vo, usdRatesTask.Result));
            return Ok(vehicleOrdersPage);
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

            vehicleOrders.ForEach(vo => UpdateOtherCurrencyPrice(vo, usdRatesTask.Result));

            return Ok(vehicleOrders);
        }

        [HttpPost]
        public IActionResult Create([FromHeader(Name = "Authorization")] string token, [FromBody] VehicleOrder vehicleOrder)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return Unauthorized();

            Customer customer = _dataAccessProvider.CustomerRepository.Get(vehicleOrder.CustomerId);
            if (customer is null)
                return NotFound("Customer not found!");

            vehicleOrder.UserId = userId;
            _dataAccessProvider.VehicleOrderRepository.Create(vehicleOrder);
            _dataAccessProvider.Save();
            
            return Created("", null);
        }

        [HttpPut]
        public IActionResult Update([FromHeader(Name = "Authorization")] string token, [FromBody] VehicleOrder updatedVehicleOrder)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return Unauthorized();

            Customer customer = _dataAccessProvider.CustomerRepository.Get(updatedVehicleOrder.CustomerId);
            if (customer is null)
                return NotFound("Customer not found!");

            VehicleOrder vehicleOrderToUpdate = _dataAccessProvider.VehicleOrderRepository.Get(updatedVehicleOrder.Id);
            if (vehicleOrderToUpdate is null)
                return NotFound("Vehicle order not found!");

            if (vehicleOrderToUpdate.UserId != userId)
                return Unauthorized("Only user assigned to an order can update that order!");

            vehicleOrderToUpdate.OrderDate = updatedVehicleOrder.OrderDate;
            vehicleOrderToUpdate.OrderDateComments = updatedVehicleOrder.OrderDateComments;
            vehicleOrderToUpdate.PriceToPayUsd = updatedVehicleOrder.PriceToPayUsd;
            vehicleOrderToUpdate.ReturnDateComments = updatedVehicleOrder.ReturnDateComments;
            vehicleOrderToUpdate.Status = updatedVehicleOrder.Status;
            vehicleOrderToUpdate.VehicleId = updatedVehicleOrder.VehicleId;
            vehicleOrderToUpdate.ActualReturnDate = updatedVehicleOrder.ActualReturnDate;
            vehicleOrderToUpdate.CustomerId = updatedVehicleOrder.CustomerId;
            vehicleOrderToUpdate.ExpectedReturnDate = updatedVehicleOrder.ExpectedReturnDate;
            vehicleOrderToUpdate.FuelPercentageOnOrderDate = updatedVehicleOrder.FuelPercentageOnOrderDate;
            vehicleOrderToUpdate.FuelPercentageOnReturnDate = updatedVehicleOrder.FuelPercentageOnReturnDate;

            _dataAccessProvider.VehicleOrderRepository.Update(vehicleOrderToUpdate);
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

        private void UpdateOtherCurrencyPrice(VehicleOrder vehicleOrder, Dictionary<string, decimal> usdRates)
        {
            vehicleOrder.OtherCurrencyPrice = new Dictionary<string, decimal>();
            if (usdRates != null)
                foreach (var kv in usdRates)
                    vehicleOrder.OtherCurrencyPrice.Add(kv.Key, vehicleOrder.PriceToPayUsd * kv.Value);
        }
    }
}
