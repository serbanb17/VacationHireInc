// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using VacationHireInc.DataLayer.Models;
using VacationHireInc.BusinessLayer.Models;

namespace VacationHireInc.Tests.BusinessLayer.Logic.UserLogicTests
{
    public partial class UserLogicTests
    {
        [TestMethod]
        public void Authenticate_ReturnOk()
        {
            //arrange
            var targetUser = _usersList.First();
            var userAuthenticateModel = new UserAuthenticateModel
            {
                UserName = targetUser.UserName,
                Password = targetUser.UserName + "pw"
            };
            string expected = _jwtHelper.GetJwt(targetUser.Id);

            //act
            LogicResult<string> result = _userLogicSut.Authenticate(userAuthenticateModel);
            bool isResultOk = result.ResultCode == ResultCode.Ok
                              && result.Object == expected;

            //assert
            Assert.IsTrue(isResultOk, "Should return correct token!");
        }

        [TestMethod]
        public void Authenticate_ReturnNotFound()
        {
            //arrange
            var targetUser = _usersList.First();
            var userAuthenticateModel = new UserAuthenticateModel
            {
                UserName = targetUser.UserName + "2",
                Password = targetUser.UserName + "pw"
            };
            string expected = _jwtHelper.GetJwt(targetUser.Id);

            //act
            LogicResult<string> result = _userLogicSut.Authenticate(userAuthenticateModel);
            bool isResultOk = result.ResultCode == ResultCode.NotFound;

            //assert
            Assert.IsTrue(isResultOk, "Should return not found username!");
        }

        [TestMethod]
        public void Authenticate_ReturnUnauthorized()
        {
            //arrange
            var targetUser = _usersList.First();
            var userAuthenticateModel = new UserAuthenticateModel
            {
                UserName = targetUser.UserName,
                Password = targetUser.UserName + "pw2"
            };
            string expected = _jwtHelper.GetJwt(targetUser.Id);

            //act
            LogicResult<string> result = _userLogicSut.Authenticate(userAuthenticateModel);
            bool isResultOk = result.ResultCode == ResultCode.Unauthorized;

            //assert
            Assert.IsTrue(isResultOk, "Should return not unauthorized!");
        }
    }
}
