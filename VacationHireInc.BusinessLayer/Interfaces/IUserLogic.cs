// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;
using System.Collections.Generic;
using VacationHireInc.BusinessLayer.Models;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.BusinessLayer.Interfaces
{
    public interface IUserLogic
    {
        public LogicResult<UserGetModel> Get(Guid id);

        public LogicResult<UserGetModel> GetByUserName(string username);

        public LogicResult<UserGetModel> GetByEmail(string email);

        public LogicResult<List<UserGetModel>> GetByPrivilege(Privilege privilege);

        public LogicResult<List<string[]>> GetPrivileges();

        public LogicResult<int> GetCount();

        public LogicResult<List<UserGetModel>> GetPage(int pageid, int pagesize);

        public LogicResult<string> Authenticate(UserAuthenticateModel user);

        public LogicResult<object> Create(string token, UserCreateModel newUser);

        public LogicResult<Guid> Update(string token, UserUpdateModel updatedUser);

        public LogicResult<Guid> Delete(string token, Guid id);

    }
}
