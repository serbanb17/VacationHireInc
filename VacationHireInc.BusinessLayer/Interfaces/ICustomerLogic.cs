// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using System.Collections.Generic;
using VacationHireInc.BusinessLayer.Models;

namespace VacationHireInc.BusinessLayer.Interfaces
{
    public interface ICustomerLogic
    {
        public LogicResult<CustomerGetModel> Get(Guid id);

        public LogicResult<List<CustomerGetModel>> GetByUserName(string name, string surname);

        public LogicResult<CustomerGetModel> GetByEmail(string email);

        public LogicResult<CustomerGetModel> GetByPhoneNumber(string phoneNumber);

        public LogicResult<int> GetCount();

        public LogicResult<List<CustomerGetModel>> GetPage(int pageId, int pageSize);

        public LogicResult<object> Create(string token, CustomerCreateModel newCustomer);

        public LogicResult<Guid> Update(string token, CustomerUpdateModel updatedCustomer);

        public LogicResult<Guid> Delete(string token, Guid id);
    }
}
