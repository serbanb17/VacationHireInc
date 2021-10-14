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
using VacationHireInc.WebApi.Models;

namespace VacationHireInc.WebApi.Controllers
{
    [Route("api/v1/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IDataAccessProvider _dataAccessProvider;
        private readonly IJwtHelper _jwtHelper;

        public CustomerController(IDataAccessProvider dataAccessProvider, IJwtHelper jwtHelper)
        {
            _dataAccessProvider = dataAccessProvider;
            _jwtHelper = jwtHelper;
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] Guid id)
        {
            Customer customer = _dataAccessProvider.CustomerRepository.Get(id);
            if (customer is null)
                return NotFound();
            var customerGetModel = new CustomerGetModel(customer);
            return Ok(customerGetModel);
        }

        [HttpGet("search/{name}/{surname}")]
        public IActionResult GetByUserName([FromRoute] string name, string surname)
        {
            List<Customer> customers = _dataAccessProvider.CustomerRepository.GetByCondition(c => 
                c.Name.ToLower().Contains(name.ToLower()) 
                && c.Surname.ToLower().Contains(surname.ToLower())
            ).ToList();
            var customerGetModels = customers.Select(c => new CustomerGetModel(c)).ToList();
            return Ok(customerGetModels);
        }

        [HttpGet("byemail/{email}")]
        public IActionResult GetByEmail([FromRoute] string email)
        {
            Customer customer = _dataAccessProvider.CustomerRepository.GetByCondition(c => c.Email == email).FirstOrDefault();
            if (customer is null)
                return NotFound();
            var customerGetModel = new CustomerGetModel(customer);
            return Ok(customerGetModel);
        }

        [HttpPost("byphonenumber")]
        public IActionResult GetByPhoneNumber([FromBody] string phoneNumber)
        {
            Customer customer = _dataAccessProvider.CustomerRepository.GetByCondition(c => c.PhoneNumber == phoneNumber).FirstOrDefault();
            if (customer is null)
                return NotFound();
            var customerGetModel = new CustomerGetModel(customer);
            return Ok(customerGetModel);
        }

        [HttpGet("count")]
        public IActionResult GetCount()
        {
            int customerCount = _dataAccessProvider.CustomerRepository.GetCount();
            return Ok(customerCount);
        }

        [HttpGet("page/{pageId}/{pageSize}")]
        public IActionResult GetPage([FromRoute] int pageId, [FromRoute] int pageSize)
        {
            List<Customer> customersPage = _dataAccessProvider.CustomerRepository.GetPage(pageId, pageSize).ToList();
            var customerGetModels = customersPage.Select(c => new CustomerGetModel(c)).ToList();
            return Ok(customerGetModels);
        }

        [HttpPost]
        public IActionResult Create([FromHeader(Name = "Authorization")] string token, [FromBody] CustomerCreateModel newCustomer)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return Unauthorized();

            newCustomer.Email = newCustomer.Email.ToLower();
            Customer duplicateCustomer = _dataAccessProvider.CustomerRepository.GetByCondition(c => c.Email == newCustomer.Email || c.PhoneNumber == newCustomer.PhoneNumber).FirstOrDefault();
            if(duplicateCustomer != null)
                return BadRequest("A customer with same email and/or phone number exists!");

            _dataAccessProvider.CustomerRepository.Create(newCustomer.GetCustomer());
            _dataAccessProvider.Save();

            return Created("", null);
        }

        [HttpPut]
        public IActionResult Update([FromHeader(Name = "Authorization")] string token, [FromBody] CustomerUpdateModel updatedCustomer)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return Unauthorized();

            updatedCustomer.Email = updatedCustomer.Email.ToLower();
            Customer duplicateCustomer = _dataAccessProvider.CustomerRepository.GetByCondition(c => 
                c.Id != updatedCustomer.Id 
                && (c.Email == updatedCustomer.Email || c.PhoneNumber == updatedCustomer.PhoneNumber)
            ).FirstOrDefault();
            if (duplicateCustomer != null)
                return BadRequest("A customer with same email and/or phone number exists!");

            Customer customerToUpdate = _dataAccessProvider.CustomerRepository.Get(updatedCustomer.Id);
            if (customerToUpdate == null)
                return BadRequest("Could not find customer to update!");

            _dataAccessProvider.CustomerRepository.Update(updatedCustomer.SetCustomer(customerToUpdate));
            _dataAccessProvider.Save();

            return Ok(customerToUpdate.Id);
        }

        [HttpDelete]
        public IActionResult Delete([FromHeader(Name = "Authorization")] string token, [FromBody] Guid id)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return Unauthorized();

            Customer customerToDelete = _dataAccessProvider.CustomerRepository.Get(id);
            if (customerToDelete == null)
                return BadRequest("Could not find customer to delete!");

            _dataAccessProvider.CustomerRepository.Delete(new Customer { Id = id });
            _dataAccessProvider.Save();

            return Ok(id);
        }
    }
}
