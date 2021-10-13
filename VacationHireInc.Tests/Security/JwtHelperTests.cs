// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Threading.Tasks;
using VacationHireInc.Security;
using VacationHireInc.Security.Interfaces;

namespace VacationHireInc.Tests.Security
{
    [TestClass]
    public class JwtHelperTests
    {
        private byte[] _key;
        private string _issuer;
        private uint _expirationSeconds;
        private IJwtHelper _jwtHelperSut;
        private Guid _inputId;

        [TestInitialize]
        public void Initialize()
        {
            _key = Encoding.Unicode.GetBytes("dqPxuDY5WGF6Bg0cSlSAzjJsqPuAf6Qc");
            _issuer = "VacationHireInc.Tests";
            _expirationSeconds = 5;
            _jwtHelperSut = new JwtHelper(_key, _issuer, _expirationSeconds);
            _inputId = new Guid("3cb0ae2114934c9988c8ccb34b660c0a");
        }

        [TestCleanup]
        public void Cleanup()
        {
            _key = null;
            _issuer = null;
            _expirationSeconds = 0;
            _jwtHelperSut = null;
            _inputId = new Guid();
        }

        [TestMethod]
        public void IsJwtValid_NoBearer_ReturnTrue()
        {
            //arrange
            
            string token = _jwtHelperSut.GetJwt(_inputId);

            //act
            bool isValid = _jwtHelperSut.IsJwtValid(token, false, out _);

            //assert
            Assert.IsTrue(isValid, "Token should be valid!");
        }

        [TestMethod]
        public void IsJwtValid_Bearer_ReturnTrue()
        {
            //arrange
            string token = _jwtHelperSut.GetJwt(_inputId);

            //act
            bool isValid = _jwtHelperSut.IsJwtValid("Bearer " + token, true, out _);

            //assert
            Assert.IsTrue(isValid, "Token should be valid!");
        }

        [TestMethod]
        public void IsJwtValid_NoBearer_ReturnFalse()
        {
            //arrange
            string token = _jwtHelperSut.GetJwt(_inputId);

            //act
            bool isValid = _jwtHelperSut.IsJwtValid("Bearer " + token, false, out _);

            //assert
            Assert.IsFalse(isValid, "Token should be invalid!");
        }

        [TestMethod]
        public void IsJwtValid_Invalid_ReturnFalse()
        {
            //arrange
            string invalidToken = "invalidToken";

            //act
            bool isValid = _jwtHelperSut.IsJwtValid(invalidToken, true, out _);

            //assert
            Assert.IsFalse(isValid, "Token should be invalid!");
        }

        [TestMethod]
        public async Task IsJwtValid_Expired_ReturnFalse()
        {
            //arrange
            string token = _jwtHelperSut.GetJwt(_inputId);

            //act
            await Task.Delay(((int)_expirationSeconds + 1) * 1000);
            bool isValid = _jwtHelperSut.IsJwtValid(token, true, out _);

            //assert
            Assert.IsFalse(isValid, "Token should be invalid!");
        }

        [TestMethod]
        public void IsJwtValid_ReturnCorrectId()
        {
            //arrange
            string token = _jwtHelperSut.GetJwt(_inputId);

            //act
            bool isValid = _jwtHelperSut.IsJwtValid("Bearer " + token, true, out Guid outId);

            //assert
            Assert.IsTrue(isValid, "Token should be valid!");
            Assert.IsTrue(_inputId == outId, "Input and output ids should be equal!");
        }
    }
}
