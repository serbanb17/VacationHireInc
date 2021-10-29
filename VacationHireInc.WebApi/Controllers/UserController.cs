// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.AspNetCore.Mvc;
using System;
using VacationHireInc.BusinessLayer.Interfaces;
using VacationHireInc.BusinessLayer.Logic;
using VacationHireInc.BusinessLayer.Models;
using VacationHireInc.DataLayer.Interfaces;
using VacationHireInc.DataLayer.Models;
using VacationHireInc.Security.Interfaces;
using VacationHireInc.WebApi.Extensions;

namespace VacationHireInc.WebApi.Controllers
{
    [Route("api/v1/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserLogic _userLogic;
        
        public UserController(IDataAccessProvider dataAccessProvider, IJwtHelper jwtHelper, IHashingHelper hashingHelper)
        {
            _userLogic = new UserLogic(dataAccessProvider, jwtHelper, hashingHelper);
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] Guid id)
        {
            return _userLogic.Get(id).GetActionResult();
        }

        [HttpGet("byusername/{username}")]
        public IActionResult GetByUserName([FromRoute] string username)
        {
            return _userLogic.GetByUserName(username).GetActionResult();
        }

        [HttpGet("byemail{email}")]
        public IActionResult GetByEmail([FromRoute] string email)
        {
            return _userLogic.GetByEmail(email).GetActionResult();
        }

        [HttpGet("byprivilege/{privilege}")]
        public IActionResult GetByPrivilege([FromRoute] Privilege privilege)
        {
            return _userLogic.GetByPrivilege(privilege).GetActionResult();
        }

        [HttpGet("privileges")]
        public IActionResult GetPrivileges()
        {
            return _userLogic.GetPrivileges().GetActionResult();
        }

        [HttpGet("count")]
        public IActionResult GetCount()
        {
            return _userLogic.GetCount().GetActionResult();
        }

        [HttpGet("page/{pageid}/{pagesize}")]
        public IActionResult GetPage([FromRoute] int pageid, [FromRoute] int pagesize)
        {
            return _userLogic.GetPage(pageid, pagesize).GetActionResult();
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserAuthenticateModel user)
        {
            return _userLogic.Authenticate(user).GetActionResult();
        }

        [HttpPost]
        public IActionResult Create([FromHeader(Name = "Authorization")] string token, [FromBody] UserCreateModel newUser)
        {
            return _userLogic.Create(token, newUser).GetActionResult();
        }

        [HttpPut]
        public IActionResult Update([FromHeader(Name = "Authorization")] string token, [FromBody] UserUpdateModel updatedUser)
        {
            return _userLogic.Update(token, updatedUser).GetActionResult();
        }

        [HttpDelete]
        public IActionResult Delete([FromHeader(Name = "Authorization")] string token, [FromBody] Guid id)
        {
            return _userLogic.Delete(token, id).GetActionResult();
        }
    }
}
