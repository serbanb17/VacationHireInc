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
        public void Delete_ReturnOk()
        {
            //arrange
            User user = _usersList.First(u => u.Privilege == Privilege.Admin);
            User userToDelete = _usersList.First(u => u.Id != user.Id && u.Privilege != Privilege.Admin);
            Guid idToDelete = userToDelete.Id;
            string token = _jwtHelper.GetJwt(user.Id);
            var outId = user.Id;
            _jwtHelperMock.Setup(x => x.IsJwtValid(It.IsAny<string>(), It.IsAny<bool>(), out outId)).Returns(true);

            //act
            LogicResult<Guid> result = _userLogicSut.Delete(token, idToDelete);
            bool isResultOk = result.ResultCode == ResultCode.Ok
                              && result.Object == idToDelete
                              && !_usersList.Any(u => u.Id == idToDelete);

            //assert
            _dataAccessProviderMock.Verify(x => x.Save(), Times.Once);
            Assert.IsTrue(isResultOk, "Should delete user");
        }

        [TestMethod]
        public void Delete_ReturnOkAdmin()
        {
            //arrange
            User user = _usersList.First(u => u.Privilege == Privilege.Admin);
            User userToDelete = user;
            Guid idToDelete = userToDelete.Id;
            string token = _jwtHelper.GetJwt(user.Id);
            var outId = user.Id;
            _jwtHelperMock.Setup(x => x.IsJwtValid(It.IsAny<string>(), It.IsAny<bool>(), out outId)).Returns(true);

            //act
            LogicResult<Guid> result = _userLogicSut.Delete(token, idToDelete);
            bool isResultOk = result.ResultCode == ResultCode.Ok
                              && result.Object == idToDelete
                              && !_usersList.Any(u => u.Id == idToDelete);

            //assert
            _dataAccessProviderMock.Verify(x => x.Save(), Times.Once);
            Assert.IsTrue(isResultOk, "Should delete admin user");
        }

        [TestMethod]
        public void Delete_ReturnForbidAdmin()
        {
            //arrange
            User user = _usersList.First(u => u.Privilege == Privilege.Admin);
            User userToDelete = _usersList.First(u => u.Id != user.Id && u.Privilege == Privilege.Admin);
            Guid idToDelete = userToDelete.Id;
            string token = _jwtHelper.GetJwt(user.Id);
            var outId = user.Id;
            _jwtHelperMock.Setup(x => x.IsJwtValid(It.IsAny<string>(), It.IsAny<bool>(), out outId)).Returns(true);

            //act
            LogicResult<Guid> result = _userLogicSut.Delete(token, idToDelete);
            bool isResultOk = result.ResultCode == ResultCode.Forbid && _usersList.Contains(userToDelete);

            //assert
            _dataAccessProviderMock.Verify(x => x.Save(), Times.Never);
            Assert.IsTrue(isResultOk, "Should not be able to delete admin");
        }

        [TestMethod]
        public void Delete_ReturnBadRequest()
        {
            //arrange
            User user = _usersList.First(u => u.Privilege == Privilege.Admin);
            User userToDelete = _usersList.First(u => u.Id != user.Id && u.Privilege != Privilege.Admin);
            Guid idToDelete = new Guid("6f3cb9bc67184cfa80505ab16904cd93");
            string token = _jwtHelper.GetJwt(user.Id);
            var outId = user.Id;
            _jwtHelperMock.Setup(x => x.IsJwtValid(It.IsAny<string>(), It.IsAny<bool>(), out outId)).Returns(true);

            //act
            LogicResult<Guid> result = _userLogicSut.Delete(token, idToDelete);
            bool isResultOk = result.ResultCode == ResultCode.BadRequest && _usersList.Contains(userToDelete);

            //assert
            _dataAccessProviderMock.Verify(x => x.Save(), Times.Never);
            Assert.IsTrue(isResultOk, "Should not find user to delete");
        }

        [TestMethod]
        public void Delete_ReturnForbidNotAdmin()
        {
            //arrange
            User user = _usersList.First(u => u.Privilege == Privilege.Clerk);
            User userToDelete = _usersList.First(u => u.Id != user.Id && u.Privilege != Privilege.Admin);
            Guid idToDelete = userToDelete.Id;
            string token = _jwtHelper.GetJwt(user.Id);
            var outId = user.Id;
            _jwtHelperMock.Setup(x => x.IsJwtValid(It.IsAny<string>(), It.IsAny<bool>(), out outId)).Returns(true);

            //act
            LogicResult<Guid> result = _userLogicSut.Delete(token, idToDelete);
            bool isResultOk = result.ResultCode == ResultCode.Forbid && _usersList.Contains(userToDelete);

            //assert
            _dataAccessProviderMock.Verify(x => x.Save(), Times.Never);
            Assert.IsTrue(isResultOk, "Should be forbid for non admin");
        }

        [TestMethod]
        public void Delete_ReturnUnauthorized()
        {
            //arrange
            User user = _usersList.First(u => u.Privilege == Privilege.Admin);
            User userToDelete = _usersList.First(u => u.Id != user.Id && u.Privilege != Privilege.Admin);
            Guid idToDelete = userToDelete.Id;
            string token = _jwtHelper.GetJwt(user.Id);
            var outId = user.Id;
            _jwtHelperMock.Setup(x => x.IsJwtValid(It.IsAny<string>(), It.IsAny<bool>(), out outId)).Returns(false);

            //act
            LogicResult<Guid> result = _userLogicSut.Delete(token, idToDelete);
            bool isResultOk = result.ResultCode == ResultCode.Unauthorized && _usersList.Contains(userToDelete);

            //assert
            _dataAccessProviderMock.Verify(x => x.Save(), Times.Never);
            Assert.IsTrue(isResultOk, "Should not be authorized");
        }
    }
}
