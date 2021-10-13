// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using VacationHireInc.DataLayer.Interfaces;
using VacationHireInc.DataLayer.Models;
using VacationHireInc.Security;
using VacationHireInc.Security.Interfaces;
using VacationHireInc.WebApi.Controllers;

namespace VacationHireInc.Tests.WebApi.Controllers.UserControllerTests
{
    [TestClass]
    public partial class UserControllerTests
    {
        private Mock<IRepository<User>> _userRepositoryMock;
        private Mock<IDataAccessProvider> _dataAccessProviderMock;
        private Mock<IJwtHelper> _jwtHelperMock;
        private IDataAccessProvider _dataAccessProvider;
        private IJwtHelper _jwtHelper;
        private IHashingHelper _hashingHelper;
        private UserController _userControllerSut;
        private List<User> _usersList;

        [TestInitialize]
        public void Initialize()
        {

            _userRepositoryMock = new Mock<IRepository<User>>();
            _dataAccessProviderMock = new Mock<IDataAccessProvider>();
            _jwtHelperMock = new Mock<IJwtHelper>();
            _dataAccessProvider = _dataAccessProviderMock.Object;
            _jwtHelper = _jwtHelperMock.Object;
            _hashingHelper = new HashingHelper(Encoding.Unicode.GetBytes("eZ3YPldh0JKB1lXODe3KKEjsqMFZ0krP"));
            _userControllerSut = new UserController(_dataAccessProvider, _jwtHelper, _hashingHelper);
            _usersList = GetUsersList(_hashingHelper);

            _dataAccessProviderMock.SetupGet(x => x.UserRepository).Returns(_userRepositoryMock.Object);
            _dataAccessProviderMock.Setup(x => x.Save());

            _jwtHelperMock.Setup(x => x.GetJwt(It.IsAny<Guid>())).Returns((Guid id) => id.ToString());

            _userRepositoryMock.Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns((Guid id) => _usersList.FirstOrDefault(u => u.Id == id));
            _userRepositoryMock.Setup(x => x.GetByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                               .Returns((Expression<Func<User, bool>> expr) => _usersList.AsQueryable().Where(expr));
            _userRepositoryMock.Setup(x => x.GetCount()).Returns(_usersList.Count);
            _userRepositoryMock.Setup(x => x.GetPage(It.IsAny<int>(), It.IsAny<int>()))
                               .Returns((int pageId, int pageSize) => 
                                        _usersList.OrderBy(u => u.Id).Skip(pageId * pageSize).Take(pageSize).ToList());
            _userRepositoryMock.Setup(x => x.Create(It.IsAny<User>())).Callback((User user) => _usersList.Add(user));
            _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>())).Callback((User user) => 
            {
                var userToUpdate = _usersList.FirstOrDefault(u => u.Id == user.Id);
                userToUpdate.Name = user.Name;
                userToUpdate.Surname = user.Surname;
                userToUpdate.UserName = user.UserName;
                userToUpdate.Email = user.Email;
                userToUpdate.Password = user.Password;
                userToUpdate.Privilege = user.Privilege;
            });
            _userRepositoryMock.Setup(x => x.Delete(It.IsAny<User>())).Callback((User user) => _usersList.RemoveAll(u => u.Id == user.Id));
        }

        [TestCleanup]
        public void Cleanup()
        {
            _userRepositoryMock = null;
            _dataAccessProviderMock = null;
            _jwtHelperMock = null;
            _dataAccessProvider = null;
            _jwtHelper = null;
            _hashingHelper = null;
            _userControllerSut = null;
            _usersList = null;
        }

        private List<User> GetUsersList(IHashingHelper hashingHelper) => new List<User> {
                new User
                {
                    Id = new Guid("2ab9ce5e27c24d82b87e922f0e9348a7"),
                    Name = "Keegan",
                    Surname = "Aguirre",
                    UserName = "keeganaguirre",
                    Email = "keeganaguirre@vacationhireinc.com",
                    Password = _hashingHelper.SaltHash("keeganaguirrepw"),
                    Privilege = Privilege.Admin
                },
                new User
                {
                    Id = new Guid("807e8c0ea5df49dc9c9b1e74e204fb75"),
                    Name = "Cleveland",
                    Surname = "Whitley",
                    UserName = "clevelandwhitley",
                    Email = "clevelandwhitley@vacationhireinc.com",
                    Password = _hashingHelper.SaltHash("clevelandwhitleypw"),
                    Privilege = Privilege.Admin
                },
                new User
                {
                    Id = new Guid("688441076ed4469093137bedf44f23f1"),
                    Name = "Letitia ",
                    Surname = "Gough",
                    UserName = "letitiagough",
                    Email = "letitiagough@vacationhireinc.com",
                    Password = _hashingHelper.SaltHash("letitiagoughpw"),
                    Privilege = Privilege.Clerk
                },
                new User
                {
                    Id = new Guid("fe156ebab926437cb6d8472c7f23241c"),
                    Name = "Zayden",
                    Surname = "Wong",
                    UserName = "zaydenwong",
                    Email = "zaydenwong@vacationhireinc.com",
                    Password = _hashingHelper.SaltHash("zaydenwongpw"),
                    Privilege = Privilege.Clerk
                }
        };
    }
}
