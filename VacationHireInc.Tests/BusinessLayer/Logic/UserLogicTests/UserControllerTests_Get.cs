// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using VacationHireInc.DataLayer.Models;
using VacationHireInc.BusinessLayer.Models;

namespace VacationHireInc.Tests.BusinessLayer.Logic.UserLogicTests
{
    public partial class UserLogicTests
    {
        #region Get

        [TestMethod]
        public void Get_ReturnOk()
        {
            //arrange
            var ids = _usersList.Select(u => u.Id).ToList();

            //act
            bool allReturnsCorrect = true;
            foreach (Guid id in ids)
            {
                LogicResult<UserGetModel> result = _userLogicSut.Get(id);
                allReturnsCorrect &= result.ResultCode == ResultCode.Ok
                                     && result.Object.Id == id;
            }

            //assert
            Assert.IsTrue(allReturnsCorrect, "All expected users should be retrieved!");
        }

        [TestMethod]
        public void Get_ReturnNotFound()
        {
            //arrange
            var id = new Guid("774809d21839469e9bf57d3177770ae5");

            //act
            LogicResult<UserGetModel> result = _userLogicSut.Get(id);
            bool notFound = result.ResultCode == ResultCode.NotFound;

            //assert
            Assert.IsTrue(notFound, "Should return not found!");
        }

        #endregion Get

        #region GetByUserName

        [TestMethod]
        public void GetByUserName_ReturnOk()
        {
            //arrange
            var usernames = _usersList.Select(u => u.UserName).ToList();

            //act
            bool allReturnsCorrect = true;
            foreach (string username in usernames)
            {
                LogicResult<UserGetModel> result = _userLogicSut.GetByUserName(username);
                allReturnsCorrect &= result.ResultCode == ResultCode.Ok
                                     && result.Object.UserName == username;
            }

            //assert
            Assert.IsTrue(allReturnsCorrect, "All expected users should be retrieved!");
        }

        [TestMethod]
        public void GetByUserName_ReturnNotFound()
        {
            //arrange
            string username = "doesnotexist";

            //act
            LogicResult<UserGetModel> result = _userLogicSut.GetByUserName(username);
            bool notFound = result.ResultCode == ResultCode.NotFound;

            //assert
            Assert.IsTrue(notFound, "Should return not found!");
        }

        #endregion GetByUserName

        #region GetByEmail

        [TestMethod]
        public void GetByEmail_ReturnOk()
        {
            //arrange
            var emails = _usersList.Select(u => u.Email).ToList();

            //act
            bool allReturnsCorrect = true;
            foreach (string email in emails)
            {
                LogicResult<UserGetModel> result = _userLogicSut.GetByEmail(email);
                allReturnsCorrect &= result.ResultCode == ResultCode.Ok
                                     && result.Object.Email == email;
            }

            //assert
            Assert.IsTrue(allReturnsCorrect, "All expected users should be retrieved!");
        }

        [TestMethod]
        public void GetByEmail_ReturnNotFound()
        {
            //arrange
            string email = "does@not.exist";

            //act
            LogicResult<UserGetModel> result = _userLogicSut.GetByEmail(email);
            bool notFound = result.ResultCode == ResultCode.NotFound;

            //assert
            Assert.IsTrue(notFound, "Should return not found!");
        }

        #endregion GetByEmail

        #region GetByPrivilege

        [TestMethod]
        public void GetByPrivilege_ReturnOk()
        {
            //arrange
            var privilege = Privilege.Admin;

            //act
            LogicResult<List<UserGetModel>> result = _userLogicSut.GetByPrivilege(privilege);
            bool isResultOk = result.ResultCode == ResultCode.Ok
                              && result.Object.Count == _usersList.Count(u => u.Privilege == privilege)
                              && result.Object.All(u => _usersList.Any(x => x.Id == u.Id));
            //assert
            Assert.IsTrue(isResultOk, "All expected users should be retrieved!");
        }

        #endregion GetByPrivilege

        #region GetCount

        [TestMethod]
        public void GetCount_ReturnOk()
        {
            //arrange
            //act
            LogicResult<int> result = _userLogicSut.GetCount();
            bool isResultOk = result.ResultCode == ResultCode.Ok
                              && result.Object == _usersList.Count;

            //assert
            Assert.IsTrue(isResultOk, "Expected count should be retrieved!");
        }

        #endregion GetCount

        #region GetPage

        [TestMethod]
        public void GetPage_ReturnOk()
        {
            //arrange
            int pageId = 1;
            int pageSize = 2;
            var expected = _usersList.OrderBy(u => u.Id).Skip(pageId * pageSize).Take(pageSize).ToList();

            //act
            LogicResult<List<UserGetModel>> result = _userLogicSut.GetPage(pageId, pageSize);
            bool isResultOk = result.ResultCode == ResultCode.Ok
                              && result.Object.Count == expected.Count
                              && result.Object.Select((u, idx) => expected[idx].Id == u.Id).All(b => b);

            //assert
            Assert.IsTrue(isResultOk, "Expected count should be retrieved!");
        }

        #endregion GetPage
    }
}
