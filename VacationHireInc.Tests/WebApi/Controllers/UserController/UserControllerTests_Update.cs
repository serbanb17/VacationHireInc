// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.Tests.WebApi.Controllers.UserControllerTests
{
    public partial class UserControllerTests
    {
        [TestMethod]
        public void Update_ReturnOk()
        {
            //arrange
            User user = _usersList.First(u => u.Privilege == Privilege.Admin);
            User userToUpdate = _usersList.First(u => u.Id != user.Id && u.Privilege != Privilege.Admin);
            string token = _jwtHelper.GetJwt(user.Id);
            var updatedUser = new User
            {
                Id = userToUpdate.Id,
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
            IActionResult result = _userControllerSut.Update(token, updatedUser);
            bool isResultOk = result is OkObjectResult resultObj
                              && resultObj.Value is Guid id
                              && id == userToUpdate.Id
                              && updatedUser.Name == userToUpdate.Name
                              && updatedUser.Surname == userToUpdate.Surname
                              && updatedUser.UserName == userToUpdate.UserName
                              && updatedUser.Email == userToUpdate.Email
                              && _hashingHelper.SaltHash(updatedUser.Password) == userToUpdate.Password
                              && updatedUser.Privilege == userToUpdate.Privilege;

            //assert
            _dataAccessProviderMock.Verify(x => x.Save(), Times.Once);
            Assert.IsTrue(isResultOk, "Should update user");
        }

        [TestMethod]
        public void Update_ReturnUnauthorized()
        {
            //arrange
            User user = _usersList.First(u => u.Privilege == Privilege.Admin);
            User userToUpdate = _usersList.First(u => u.Id != user.Id);
            string token = _jwtHelper.GetJwt(user.Id);
            var updatedUser = new User
            {
                Id = userToUpdate.Id,
                Name = "Mikayla",
                Surname = "Vargas",
                UserName = "mikaylavargas",
                Email = "mikaylavargas@vacationhireinc.com",
                Password = "mikaylavargaspw",
                Privilege = Privilege.Clerk
            };
            var outId = user.Id;
            _jwtHelperMock.Setup(x => x.IsJwtValid(It.IsAny<string>(), It.IsAny<bool>(), out outId)).Returns(false);

            //act
            IActionResult result = _userControllerSut.Update(token, updatedUser);
            bool isResultOk = result is UnauthorizedResult;

            //assert
            _dataAccessProviderMock.Verify(x => x.Save(), Times.Never);
            Assert.IsTrue(isResultOk, "Should return unauthorized");
        }

        [TestMethod]
        public void Update_ReturnForbid()
        {
            //arrange
            User user = _usersList.First(u => u.Privilege == Privilege.Clerk);
            User userToUpdate = _usersList.First(u => u.Id != user.Id);
            string token = _jwtHelper.GetJwt(user.Id);
            var updatedUser = new User
            {
                Id = userToUpdate.Id,
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
            IActionResult result = _userControllerSut.Update(token, updatedUser);
            bool isResultOk = result is ForbidResult;

            //assert
            _dataAccessProviderMock.Verify(x => x.Save(), Times.Never);
            Assert.IsTrue(isResultOk, "Should return forbid");
        }

        [TestMethod]
        public void Update_ReturnBadRequest()
        {
            //arrange
            User user = _usersList.First(u => u.Privilege == Privilege.Admin);
            User userToUpdate = _usersList.First(u => u.Id != user.Id);
            string token = _jwtHelper.GetJwt(user.Id);
            var updatedUser = new User
            {
                Id = new Guid("961b96e2e7524658b70cef31d9e2a707"),
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
            IActionResult result = _userControllerSut.Update(token, updatedUser);
            bool isResultOk = result is BadRequestObjectResult;

            //assert
            _dataAccessProviderMock.Verify(x => x.Save(), Times.Never);
            Assert.IsTrue(isResultOk, "Should return bad request");
        }

        [TestMethod]
        public void Update_ReturnForbidUpdateAdmin()
        {
            //arrange
            User user = _usersList.First(u => u.Privilege == Privilege.Admin);
            User userToUpdate = _usersList.First(u => u.Id != user.Id && u.Privilege == Privilege.Admin);
            string token = _jwtHelper.GetJwt(user.Id);
            var updatedUser = new User
            {
                Id = userToUpdate.Id,
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
            IActionResult result = _userControllerSut.Update(token, updatedUser);
            bool isResultOk = result is ForbidResult;

            //assert
            _dataAccessProviderMock.Verify(x => x.Save(), Times.Never);
            Assert.IsTrue(isResultOk, "Should return bad request when trying to update another admin");
        }
    }
}
