// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using VacationHireInc.BusinessLayer.Models;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.Tests.BusinessLayer.Logic.UserLogicTests
{
    public partial class UserLogicTests
    {
        [TestMethod]
        public void Create_ReturnCreated()
        {
            //arrange
            User user = _usersList.First(u => u.Privilege == Privilege.Admin);
            string token = _jwtHelper.GetJwt(user.Id);
            var newUser = new UserCreateModel
            {
                Name = "Mikayla",
                Surname = "Vargas",
                UserName = "mikaylavargas",
                Email = "mikaylavargas@vacationhireinc.com",
                Password = "mikaylavargaspw",
                Privilege = Privilege.Clerk
            };
            var outId = user.Id;
            _jwtHelperMock.Setup(x => x.IsJwtValid(It.IsAny<string>(), It.IsAny<bool>(), out outId)).Returns(true);

            //act
            LogicResult<object> result = _userLogicSut.Create(token, newUser);
            bool isResultOk = result.ResultCode == ResultCode.Created
                              && _usersList.Last().Name == newUser.Name
                              && _usersList.Last().Surname == newUser.Surname
                              && _usersList.Last().UserName == newUser.UserName
                              && _usersList.Last().Email == newUser.Email
                              && _usersList.Last().Password == newUser.Password
                              && _usersList.Last().Privilege == newUser.Privilege;

            //assert
            _dataAccessProviderMock.Verify(x => x.Save(), Times.Once);
            Assert.IsTrue(isResultOk, "Should create new user");
        }

        [TestMethod]
        public void Create_ReturnUnauthorized()
        {
            //arrange
            User user = _usersList.First(u => u.Privilege == Privilege.Admin);
            string token = _jwtHelper.GetJwt(user.Id);
            var newUser = new UserCreateModel
            {
                Name = "Mikayla",
                Surname = "Vargas",
                UserName = "mikaylavargas",
                Email = "mikaylavargas@vacationhireinc.com",
                Password = "mikaylavargaspw",
                Privilege = Privilege.Clerk
            };
            var outId = new Guid("1f32fe43cee241488c655073932a9003");
            _jwtHelperMock.Setup(x => x.IsJwtValid(It.IsAny<string>(), It.IsAny<bool>(), out outId)).Returns(false);

            //act
            LogicResult<object> result = _userLogicSut.Create(token, newUser);
            bool isResultOk = result.ResultCode == ResultCode.Unauthorized;

            //assert
            _dataAccessProviderMock.Verify(x => x.Save(), Times.Never);
            Assert.IsTrue(isResultOk, "Should return unauthorized");
        }

        [TestMethod]
        public void Create_ReturnForbid()
        {
            //arrange
            User user = _usersList.First(u => u.Privilege == Privilege.Clerk);
            string token = _jwtHelper.GetJwt(user.Id);
            var newUser = new UserCreateModel
            {
                Name = "Mikayla",
                Surname = "Vargas",
                UserName = "mikaylavargas",
                Email = "mikaylavargas@vacationhireinc.com",
                Password = "mikaylavargaspw",
                Privilege = Privilege.Clerk
            };
            var outId = user.Id;
            _jwtHelperMock.Setup(x => x.IsJwtValid(It.IsAny<string>(), It.IsAny<bool>(), out outId)).Returns(true);

            //act
            LogicResult<object> result = _userLogicSut.Create(token, newUser);
            bool isResultOk = result.ResultCode == ResultCode.Forbid;

            //assert
            _dataAccessProviderMock.Verify(x => x.Save(), Times.Never);
            Assert.IsTrue(isResultOk, "Should return forbid");
        }

        [TestMethod]
        public void Create_ReturnBadRequest()
        {
            //arrange
            User user = _usersList.First(u => u.Privilege == Privilege.Admin);
            string token = _jwtHelper.GetJwt(user.Id);
            var userToAdd = _usersList.First(u => u.Id != user.Id);
            var newUser = new UserCreateModel
            {
                Name = userToAdd.Name,
                Surname = userToAdd.Surname,
                UserName = userToAdd.UserName,
                Email = userToAdd.Email,
                Password = userToAdd.Password,
                Privilege = Privilege.Clerk
            };
            var outId = user.Id;
            _jwtHelperMock.Setup(x => x.IsJwtValid(It.IsAny<string>(), It.IsAny<bool>(), out outId)).Returns(true);

            //act
            LogicResult<object> result = _userLogicSut.Create(token, newUser);
            bool isResultOk = result.ResultCode == ResultCode.BadRequest;

            //assert
            _dataAccessProviderMock.Verify(x => x.Save(), Times.Never);
            Assert.IsTrue(isResultOk, "Should return bad request");
        }
    }
}
