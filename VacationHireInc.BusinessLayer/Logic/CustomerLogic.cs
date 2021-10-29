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
    public class CustomerLogic : ICustomerLogic
    {
        private readonly IDataAccessProvider _dataAccessProvider;
        private readonly IJwtHelper _jwtHelper;

        public CustomerLogic(IDataAccessProvider dataAccessProvider, IJwtHelper jwtHelper)
        {
            _dataAccessProvider = dataAccessProvider;
            _jwtHelper = jwtHelper;
        }

        public LogicResult<CustomerGetModel> Get(Guid id)
        {
            Customer customer = _dataAccessProvider.CustomerRepository.Get(id);
            if (customer is null)
                return new LogicResult<CustomerGetModel>(ResultCode.NotFound);
            var customerGetModel = new CustomerGetModel(customer);
            return new LogicResult<CustomerGetModel>(customerGetModel, ResultCode.Ok);
        }

        public LogicResult<List<CustomerGetModel>> GetByUserName(string name, string surname)
        {
            List<Customer> customers = _dataAccessProvider.CustomerRepository.GetByCondition(c =>
                c.Name.ToLower().Contains(name.ToLower())
                && c.Surname.ToLower().Contains(surname.ToLower())
            ).ToList();
            var customerGetModels = customers.Select(c => new CustomerGetModel(c)).ToList();
            return new LogicResult<List<CustomerGetModel>>(customerGetModels, ResultCode.Ok);
        }

        public LogicResult<CustomerGetModel> GetByEmail(string email)
        {
            Customer customer = _dataAccessProvider.CustomerRepository.GetByCondition(c => c.Email == email).FirstOrDefault();
            if (customer is null)
                return new LogicResult<CustomerGetModel>(ResultCode.NotFound);
            var customerGetModel = new CustomerGetModel(customer);
            return new LogicResult<CustomerGetModel>(customerGetModel, ResultCode.Ok);
        }

        public LogicResult<CustomerGetModel> GetByPhoneNumber(string phoneNumber)
        {
            Customer customer = _dataAccessProvider.CustomerRepository.GetByCondition(c => c.PhoneNumber == phoneNumber).FirstOrDefault();
            if (customer is null)
                return new LogicResult<CustomerGetModel>(ResultCode.NotFound);
            var customerGetModel = new CustomerGetModel(customer);
            return new LogicResult<CustomerGetModel>(customerGetModel, ResultCode.Ok);
        }

        public LogicResult<int> GetCount()
        {
            int customerCount = _dataAccessProvider.CustomerRepository.GetCount();
            return new LogicResult<int>(customerCount, ResultCode.Ok);
        }

        public LogicResult<List<CustomerGetModel>> GetPage(int pageId, int pageSize)
        {
            List<Customer> customersPage = _dataAccessProvider.CustomerRepository.GetPage(pageId, pageSize).ToList();
            var customerGetModels = customersPage.Select(c => new CustomerGetModel(c)).ToList();
            return new LogicResult<List<CustomerGetModel>>(customerGetModels, ResultCode.Ok);
        }

        public LogicResult<object> Create(string token, CustomerCreateModel newCustomer)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return new LogicResult<object>(ResultCode.Unauthorized);

            newCustomer.Email = newCustomer.Email.ToLower();
            Customer duplicateCustomer = _dataAccessProvider.CustomerRepository.GetByCondition(c => c.Email == newCustomer.Email || c.PhoneNumber == newCustomer.PhoneNumber).FirstOrDefault();
            if (duplicateCustomer != null)
                return new LogicResult<object>(ResultCode.BadRequest, "A customer with same email and/or phone number exists!");

            _dataAccessProvider.CustomerRepository.Create(newCustomer.GetCustomer());
            _dataAccessProvider.Save();

            return new LogicResult<object>(null, ResultCode.Created);
        }

        public LogicResult<Guid> Update(string token, CustomerUpdateModel updatedCustomer)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return new LogicResult<Guid>(ResultCode.Unauthorized);

            updatedCustomer.Email = updatedCustomer.Email.ToLower();
            Customer duplicateCustomer = _dataAccessProvider.CustomerRepository.GetByCondition(c =>
                c.Id != updatedCustomer.Id
                && (c.Email == updatedCustomer.Email || c.PhoneNumber == updatedCustomer.PhoneNumber)
            ).FirstOrDefault();
            if (duplicateCustomer != null)
                return new LogicResult<Guid>(ResultCode.BadRequest, "A customer with same email and/or phone number exists!");

            Customer customerToUpdate = _dataAccessProvider.CustomerRepository.Get(updatedCustomer.Id);
            if (customerToUpdate == null)
                return new LogicResult<Guid>(ResultCode.BadRequest, "Could not find customer to update!");

            _dataAccessProvider.CustomerRepository.Update(updatedCustomer.SetCustomer(customerToUpdate));
            _dataAccessProvider.Save();

            return new LogicResult<Guid>(customerToUpdate.Id, ResultCode.Ok);
        }

        public LogicResult<Guid> Delete(string token, Guid id)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return new LogicResult<Guid>(ResultCode.Unauthorized);

            Customer customerToDelete = _dataAccessProvider.CustomerRepository.Get(id);
            if (customerToDelete == null)
                return new LogicResult<Guid>(ResultCode.BadRequest, "Could not find customer to delete!");

            _dataAccessProvider.CustomerRepository.Delete(new Customer { Id = id });
            _dataAccessProvider.Save();

            return new LogicResult<Guid>(id, ResultCode.Ok);
        }
    }
}
