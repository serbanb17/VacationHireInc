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
    [Route("api/v1/vehicle")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleLogic _vehicleLogic;
        private readonly ICurrencyRatesUsdProvider _currencyRatesUsdProvider;

        public VehicleController(IDataAccessProvider dataAccessProvider, IJwtHelper jwtHelper, ICurrencyRatesUsdProvider currencyRatesUsdProvider)
        {
            _vehicleLogic = new VehicleLogic(dataAccessProvider, jwtHelper);
            _currencyRatesUsdProvider = currencyRatesUsdProvider;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var usdRates = await _currencyRatesUsdProvider.GetRatesAsync();
            return _vehicleLogic.Get(id, usdRates).GetActionResult();
        }

        [HttpGet("fuelTypes")]
        public IActionResult GetFuelTypes()
        {
            return _vehicleLogic.GetFuelTypes().GetActionResult();
        }

        [HttpGet("bodyTypes")]
        public IActionResult GetBodyTypes()
        {
            return _vehicleLogic.GetBodyTypes().GetActionResult();
        }

        [HttpGet("count")]
        public IActionResult GetCount()
        {
            return _vehicleLogic.GetCount().GetActionResult();
        }

        [HttpGet("page/{pageId}/{pageSize}")]
        public async Task<IActionResult> GetPage([FromRoute] int pageId, [FromRoute] int pageSize)
        {
            var usdRates = await _currencyRatesUsdProvider.GetRatesAsync();
            return _vehicleLogic.GetPage(pageId, pageSize, usdRates).GetActionResult();
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetByFilter([FromHeader(Name = "Authorization")] string token, [FromBody] VehicleFilter vehicleFilter)
        {
            var usdRates = await _currencyRatesUsdProvider.GetRatesAsync();
            return _vehicleLogic.GetByFilter(token, vehicleFilter, usdRates).GetActionResult();
        }

        [HttpPost]
        public IActionResult Create([FromHeader(Name = "Authorization")] string token, [FromBody] VehicleCreateModel newVehicle)
        {
            return _vehicleLogic.Create(token, newVehicle).GetActionResult();
        }

        [HttpPut]
        public IActionResult Update([FromHeader(Name = "Authorization")] string token, [FromBody] VehicleUpdateModel updatedVehicle)
        {
            return _vehicleLogic.Update(token, updatedVehicle).GetActionResult();
        }

        [HttpDelete]
        public IActionResult Delete([FromHeader(Name = "Authorization")] string token, [FromBody] Guid id)
        {
            return _vehicleLogic.Delete(token, id).GetActionResult();
        }
    }
}
