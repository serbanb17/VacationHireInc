// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VacationHireInc.BusinessLayer.Interfaces;
using VacationHireInc.BusinessLayer.Logic;
using VacationHireInc.BusinessLayer.Models;
using VacationHireInc.DataLayer.Interfaces;
using VacationHireInc.Security.Interfaces;
using VacationHireInc.WebApi.Extensions;
using VacationHireInc.WebApi.Interfaces;

namespace VacationHireInc.WebApi.Controllers
{
    [Route("api/v1/vehicleorder")]
    [ApiController]
    public class VehicleOrderController : ControllerBase
    {
        private readonly IVehicleOrderLogic _vehicleOrderLogic;
        private readonly ICurrencyRatesUsdProvider _currencyRatesUsdProvider;

        public VehicleOrderController(IDataAccessProvider dataAccessProvider, IJwtHelper jwtHelper, ICurrencyRatesUsdProvider currencyRatesUsdProvider)
        {
            _vehicleOrderLogic = new VehicleOrderLogic(dataAccessProvider, jwtHelper);
            _currencyRatesUsdProvider = currencyRatesUsdProvider;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Get([FromHeader(Name = "Authorization")] string token, [FromRoute] Guid id)
        {
            var usdRates = await _currencyRatesUsdProvider.GetRatesAsync();
            return _vehicleOrderLogic.Get(token, id, usdRates).GetActionResult();
        }

        [HttpGet("count")]
        public IActionResult GetCount([FromHeader(Name = "Authorization")] string token)
        {
            return _vehicleOrderLogic.GetCount(token).GetActionResult();
        }

        [HttpGet("page/{pageId}/{pageSize}")]
        public async Task<IActionResult> GetPage([FromHeader(Name = "Authorization")] string token, [FromRoute] int pageId, [FromRoute] int pageSize)
        {
            var usdRates = await _currencyRatesUsdProvider.GetRatesAsync();
            return _vehicleOrderLogic.GetPage(token, pageId, pageSize, usdRates).GetActionResult();
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetByFilter([FromHeader(Name = "Authorization")] string token, [FromBody] VehicleOrderFilter vehicleOrderFilter)
        {
            var usdRates = await _currencyRatesUsdProvider.GetRatesAsync();
            return _vehicleOrderLogic.GetByFilter(token, vehicleOrderFilter, usdRates).GetActionResult();
        }

        [HttpPost]
        public IActionResult Create([FromHeader(Name = "Authorization")] string token, [FromBody] VehicleOrderCreateModel vehicleOrder)
        {
            return _vehicleOrderLogic.Create(token, vehicleOrder).GetActionResult();
        }

        [HttpPut]
        public IActionResult Update([FromHeader(Name = "Authorization")] string token, [FromBody] VehicleOrderUpdateModel updatedVehicleOrder)
        {
            return _vehicleOrderLogic.Update(token, updatedVehicleOrder).GetActionResult();
        }

        [HttpDelete]
        public IActionResult Delete([FromHeader(Name = "Authorization")] string token, [FromBody] Guid id)
        {
            return _vehicleOrderLogic.Delete(token, id).GetActionResult();
        }
    }
}
