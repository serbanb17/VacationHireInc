// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using VacationHireInc.DataLayer.Models;

namespace VacationHireInc.Tests.WebApi.Controllers.UserControllerTests
{
    public partial class UserControllerTests
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
                IActionResult result = _userControllerSut.Get(id);
                allReturnsCorrect &= result is OkObjectResult okResult
                                     && okResult.Value is User user
                                     && user.Id == id;
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
            IActionResult result = _userControllerSut.Get(id);
            bool notFound = result is NotFoundObjectResult;

            //assert
            Assert.IsFalse(notFound, "Should return not found!");
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
                IActionResult result = _userControllerSut.GetByUserName(username);
                allReturnsCorrect &= result is OkObjectResult okResult
                                     && okResult.Value is User user
                                     && user.UserName == username;
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
            IActionResult result = _userControllerSut.GetByUserName(username);
            bool notFound = result is NotFoundObjectResult;

            //assert
            Assert.IsFalse(notFound, "Should return not found!");
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
                IActionResult result = _userControllerSut.GetByEmail(email);
                allReturnsCorrect &= result is OkObjectResult okResult
                                     && okResult.Value is User user
                                     && user.Email == email;
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
            IActionResult result = _userControllerSut.GetByEmail(email);
            bool notFound = result is NotFoundObjectResult;

            //assert
            Assert.IsFalse(notFound, "Should return not found!");
        }

        #endregion GetByEmail

        #region GetByPrivilege

        [TestMethod]
        public void GetByPrivilege_ReturnOk()
        {
            //arrange
            var privilege = Privilege.Admin;

            //act
            IActionResult result = _userControllerSut.GetByPrivilege(privilege);
            bool isResultOk = result is OkObjectResult resultObj
                              && resultObj.Value is List<User> resultUsers
                              && resultUsers.Count == _usersList.Count(u => u.Privilege == privilege)
                              && resultUsers.All(u => _usersList.Any(x => x.Id == u.Id));

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
            IActionResult result = _userControllerSut.GetCount();
            bool isResultOk = result is OkObjectResult resultObj
                              && resultObj.Value is int count
                              && count == _usersList.Count;

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
            IActionResult result = _userControllerSut.GetPage(pageId, pageSize);
            bool isResultOk = result is OkObjectResult resultObj
                              && resultObj.Value is List<User> users
                              && users.Count == expected.Count
                              && users.Select((u, idx) => expected[idx].Id == u.Id).All(b => b);

            //assert
            Assert.IsTrue(isResultOk, "Expected count should be retrieved!");
        }

        #endregion GetPage
    }
}
