// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.AspNetCore.Mvc;
using System;
using VacationHireInc.BusinessLayer.Interfaces;
using VacationHireInc.BusinessLayer.Logic;
using VacationHireInc.BusinessLayer.Models;
using VacationHireInc.DataLayer.Interfaces;
using VacationHireInc.Security.Interfaces;
using VacationHireInc.WebApi.Extensions;

namespace VacationHireInc.WebApi.Controllers
{
    [Route("api/v1/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerLogic _customerLogic;

        public CustomerController(IDataAccessProvider dataAccessProvider, IJwtHelper jwtHelper)
        {
            _dataAccessProvider = dataAccessProvider;
            _jwtHelper = jwtHelper;
            _customerLogic = new CustomerLogic(dataAccessProvider, jwtHelper);
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] Guid id)
        {
            return _customerLogic.Get(id).GetActionResult();
        }

        [HttpGet("search/{name}/{surname}")]
        public IActionResult GetByUserName([FromRoute] string name, string surname)
        {
            return _customerLogic.GetByUserName(name, surname).GetActionResult();
        }

        [HttpGet("byemail/{email}")]
        public IActionResult GetByEmail([FromRoute] string email)
        {
            return _customerLogic.GetByEmail(email).GetActionResult();
        }

        [HttpPost("byphonenumber")]
        public IActionResult GetByPhoneNumber([FromBody] string phoneNumber)
        {
            return _customerLogic.GetByPhoneNumber(phoneNumber).GetActionResult();
        }

        [HttpGet("count")]
        public IActionResult GetCount()
        {
            return _customerLogic.GetCount().GetActionResult();
        }

        [HttpGet("page/{pageId}/{pageSize}")]
        public IActionResult GetPage([FromRoute] int pageId, [FromRoute] int pageSize)
        {
            return _customerLogic.GetPage(pageId, pageSize).GetActionResult();
        }

        [HttpPost]
        public IActionResult Create([FromHeader(Name = "Authorization")] string token, [FromBody] CustomerCreateModel newCustomer)
        {
            return _customerLogic.Create(token, newCustomer).GetActionResult();
        }

        [HttpPut]
        public IActionResult Update([FromHeader(Name = "Authorization")] string token, [FromBody] CustomerUpdateModel updatedCustomer)
        {
            return _customerLogic.Update(token, updatedCustomer).GetActionResult();
        }

        [HttpDelete]
        public IActionResult Delete([FromHeader(Name = "Authorization")] string token, [FromBody] Guid id)
        {
            return _customerLogic.Delete(token, id).GetActionResult();
        }
    }
}
