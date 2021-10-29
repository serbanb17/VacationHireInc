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
    public class UserLogic : IUserLogic
    {
        private readonly IDataAccessProvider _dataAccessProvider;
        private readonly IJwtHelper _jwtHelper;
        private readonly IHashingHelper _hashingHelper;

        public UserLogic(IDataAccessProvider dataAccessProvider, IJwtHelper jwtHelper, IHashingHelper hashingHelper)
        {
            _dataAccessProvider = dataAccessProvider;
            _jwtHelper = jwtHelper;
            _hashingHelper = hashingHelper;
        }

        public LogicResult<UserGetModel> Get(Guid id)
        {
            User user = _dataAccessProvider.UserRepository.Get(id);
            if (user is null)
                return new LogicResult<UserGetModel>(ResultCode.NotFound);
            var userGetModel = new UserGetModel(user);
            return new LogicResult<UserGetModel>(userGetModel, ResultCode.Ok);
        }

        public LogicResult<UserGetModel> GetByUserName(string username)
        {
            User user = _dataAccessProvider.UserRepository.GetByCondition(c => c.UserName == username).FirstOrDefault();
            if (user is null)
                return new LogicResult<UserGetModel>(ResultCode.NotFound);
            var userGetModel = new UserGetModel(user);
            return new LogicResult<UserGetModel>(userGetModel, ResultCode.Ok);
        }

        public LogicResult<UserGetModel> GetByEmail(string email)
        {
            User user = _dataAccessProvider.UserRepository.GetByCondition(c => c.Email == email).FirstOrDefault();
            if (user is null)
                return new LogicResult<UserGetModel>(ResultCode.NotFound);
            var userGetModel = new UserGetModel(user);
            return new LogicResult<UserGetModel>(userGetModel, ResultCode.Ok);
        }

        public LogicResult<List<UserGetModel>> GetByPrivilege(Privilege privilege)
        {
            List<User> users = _dataAccessProvider.UserRepository.GetByCondition(c => c.Privilege == privilege).ToList();
            users.ForEach(u => u.Password = string.Empty);
            var userGetModels = users.Select(u => new UserGetModel(u)).ToList();
            return new LogicResult<List<UserGetModel>>(userGetModels, ResultCode.Ok);
        }

        public LogicResult<List<string[]>> GetPrivileges()
        {
            List<string[]> privileges = Enum.GetValues(typeof(Privilege)).Cast<Privilege>().Select(p => new[] { ((int)p).ToString(), p.ToString() }).ToList();
            return new LogicResult<List<string[]>>(privileges, ResultCode.Ok);
        }

        public LogicResult<int> GetCount()
        {
            int userCount = _dataAccessProvider.UserRepository.GetCount();
            return new LogicResult<int>(userCount, ResultCode.Ok);
        }

        public LogicResult<List<UserGetModel>> GetPage(int pageid, int pagesize)
        {
            List<User> usersPage = _dataAccessProvider.UserRepository.GetPage(pageid, pagesize).ToList();
            var userGetModels = usersPage.Select(u => new UserGetModel(u)).ToList();
            return new LogicResult<List<UserGetModel>>(userGetModels, ResultCode.Ok);
        }

        public LogicResult<string> Authenticate(UserAuthenticateModel user)
        {
            User dbUser = _dataAccessProvider.UserRepository.GetByCondition(u => u.UserName == user.UserName)?.FirstOrDefault();
            if (dbUser is null)
                return new LogicResult<string>(ResultCode.NotFound, $"UserName not found!");

            string hashedPassword = _hashingHelper.SaltHash(user.Password);
            if (hashedPassword != dbUser.Password)
                return new LogicResult<string>(ResultCode.Unauthorized, $"Wrong password!");

            string token = _jwtHelper.GetJwt(dbUser.Id);
            return new LogicResult<string>(token, ResultCode.Ok);
        }

        public LogicResult<object> Create(string token, UserCreateModel newUser)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return new LogicResult<object>(ResultCode.Unauthorized);

            User user = _dataAccessProvider.UserRepository.Get(userId);
            if (user.Privilege != Privilege.Admin)
                return new LogicResult<object>(ResultCode.Forbid, "Only allowed for admin privileged user!");

            newUser.UserName = newUser.UserName.ToLower();
            newUser.Email = newUser.Email.ToLower();
            User duplicateUser = _dataAccessProvider.UserRepository.GetByCondition(u => u.UserName == newUser.UserName || u.Email == newUser.Email).FirstOrDefault();
            if (duplicateUser != null)
                return new LogicResult<object>(ResultCode.BadRequest, "A user with same email and/or password exists!");

            string hashedPassword = _hashingHelper.SaltHash(newUser.Password);
            newUser.Password = hashedPassword;
            _dataAccessProvider.UserRepository.Create(newUser.GetUser());
            _dataAccessProvider.Save();

            return new LogicResult<object>(null, ResultCode.Created, string.Empty);
        }

        public LogicResult<Guid> Update(string token, UserUpdateModel updatedUser)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return new LogicResult<Guid>(ResultCode.Unauthorized);

            User user = _dataAccessProvider.UserRepository.Get(userId);
            if (user.Privilege != Privilege.Admin)
                return new LogicResult<Guid>(ResultCode.Forbid, "Only allowed for admin privileged user!");

            User duplicateUser = _dataAccessProvider.UserRepository.GetByCondition(u =>
                u.Id != updatedUser.Id
                && (u.UserName == updatedUser.UserName || u.Email == updatedUser.Email)
            ).FirstOrDefault();
            if (duplicateUser != null)
                return new LogicResult<Guid>(ResultCode.BadRequest, "A user with same email and/or password exists!");

            updatedUser.UserName = updatedUser.UserName.ToLower();
            updatedUser.Email = updatedUser.Email.ToLower();

            User userToUpdate = _dataAccessProvider.UserRepository.Get(updatedUser.Id);
            if (userToUpdate == null)
                return new LogicResult<Guid>(ResultCode.BadRequest, "Could not find user to update!");

            if (userToUpdate.Privilege == Privilege.Admin && user.Id != userToUpdate.Id)
                return new LogicResult<Guid>(ResultCode.Forbid, "Admin privileged users can only be updated by themselves!");

            string hashedPassword = _hashingHelper.SaltHash(updatedUser.Password);
            updatedUser.Password = hashedPassword;

            _dataAccessProvider.UserRepository.Update(updatedUser.SetUser(userToUpdate));
            _dataAccessProvider.Save();

            return new LogicResult<Guid>(userToUpdate.Id, ResultCode.Ok);
        }

        public LogicResult<Guid> Delete(string token, Guid id)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return new LogicResult<Guid>(ResultCode.Unauthorized);

            User user = _dataAccessProvider.UserRepository.Get(userId);
            if (user.Privilege != Privilege.Admin)
                return new LogicResult<Guid>(ResultCode.Forbid, "Only allowed for admin privileged user!");

            User userToDelete = _dataAccessProvider.UserRepository.Get(id);
            if (userToDelete == null)
                return new LogicResult<Guid>(ResultCode.BadRequest, "Could not find user to delete!");

            if (userToDelete.Privilege == Privilege.Admin && userToDelete.Id != userId)
                return new LogicResult<Guid>(ResultCode.Forbid, "Admin privileged users can only be deleted by themselves!");

            _dataAccessProvider.UserRepository.Delete(new User { Id = id });
            _dataAccessProvider.Save();

            return new LogicResult<Guid>(id, ResultCode.Ok);
        }
    }
}
