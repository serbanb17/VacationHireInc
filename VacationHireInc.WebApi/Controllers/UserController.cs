// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using VacationHireInc.DataLayer.Interfaces;
using VacationHireInc.DataLayer.Models;
using VacationHireInc.Security.Interfaces;
using VacationHireInc.WebApi.Models;

namespace VacationHireInc.WebApi.Controllers
{
    [Route("api/v1/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IDataAccessProvider _dataAccessProvider;
        private readonly IJwtHelper _jwtHelper;
        private readonly IHashingHelper _hashingHelper;

        public UserController(IDataAccessProvider dataAccessProvider, IJwtHelper jwtHelper, IHashingHelper hashingHelper)
        {
            _dataAccessProvider = dataAccessProvider;
            _jwtHelper = jwtHelper;
            _hashingHelper = hashingHelper;
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] Guid id)
        {
            User user = _dataAccessProvider.UserRepository.Get(id);
            if (user is null)
                return NotFound();
            var userGetModel = new UserGetModel(user);
            return Ok(userGetModel);
        }

        [HttpGet("byusername/{username}")]
        public IActionResult GetByUserName([FromRoute] string username)
        {
            User user = _dataAccessProvider.UserRepository.GetByCondition(c => c.UserName == username).FirstOrDefault();
            if (user is null)
                return NotFound();
            var userGetModel = new UserGetModel(user);
            return Ok(userGetModel);
        }

        [HttpGet("byemail{email}")]
        public IActionResult GetByEmail([FromRoute] string email)
        {
            User user = _dataAccessProvider.UserRepository.GetByCondition(c => c.Email == email).FirstOrDefault();
            if (user is null)
                return NotFound();
            var userGetModel = new UserGetModel(user);
            return Ok(userGetModel);
        }

        [HttpGet("byprivilege/{privilege}")]
        public IActionResult GetByPrivilege([FromRoute] Privilege privilege)
        {
            List<User> users = _dataAccessProvider.UserRepository.GetByCondition(c => c.Privilege == privilege).ToList();
            users.ForEach(u => u.Password = string.Empty);
            var userGetModels = users.Select(u => new UserGetModel(u)).ToList();
            return Ok(userGetModels);
        }

        [HttpGet("privileges")]
        public IActionResult GetPrivileges()
        {
            List<string[]> privileges = Enum.GetValues(typeof(Privilege)).Cast<Privilege>().Select(p => new[] { ((int)p).ToString(), p.ToString() }).ToList();
            return Ok(privileges);
        }

        [HttpGet("count")]
        public IActionResult GetCount()
        {
            int userCount = _dataAccessProvider.UserRepository.GetCount();
            return Ok(userCount);
        }

        [HttpGet("page/{pageid}/{pagesize}")]
        public IActionResult GetPage([FromRoute] int pageid, [FromRoute] int pagesize)
        {
            List<User> usersPage = _dataAccessProvider.UserRepository.GetPage(pageid, pagesize).ToList();
            var userGetModels = usersPage.Select(u => new UserGetModel(u)).ToList();
            return Ok(userGetModels);
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserAuthenticateModel user)
        {
            User dbUser = _dataAccessProvider.UserRepository.GetByCondition(u => u.UserName == user.UserName)?.FirstOrDefault();
            if (dbUser is null)
                return NotFound($"UserName not found!");

            string hashedPassword = _hashingHelper.SaltHash(user.Password);
            if (hashedPassword != dbUser.Password)
                return Unauthorized($"Wrong password!");

            string token = _jwtHelper.GetJwt(dbUser.Id);
            return Ok(token);
        }

        [HttpPost]
        public IActionResult Create([FromHeader(Name = "Authorization")] string token, [FromBody] UserCreateModel newUser)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return Unauthorized();

            User user = _dataAccessProvider.UserRepository.Get(userId);
            if (user.Privilege != Privilege.Admin)
                return Forbid("Only allowed for admin privileged user!");

            newUser.UserName = newUser.UserName.ToLower();
            newUser.Email = newUser.Email.ToLower();
            User duplicateUser = _dataAccessProvider.UserRepository.GetByCondition(u => u.UserName == newUser.UserName || u.Email == newUser.Email).FirstOrDefault();
            if (duplicateUser != null)
                return BadRequest("A user with same email and/or password exists!");

            string hashedPassword = _hashingHelper.SaltHash(newUser.Password);
            newUser.Password = hashedPassword;
            _dataAccessProvider.UserRepository.Create(newUser.GetUser());
            _dataAccessProvider.Save();

            return Created("", null);
        }

        [HttpPut]
        public IActionResult Update([FromHeader(Name = "Authorization")] string token, [FromBody] UserUpdateModel updatedUser)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return Unauthorized();

            User user = _dataAccessProvider.UserRepository.Get(userId);
            if (user.Privilege != Privilege.Admin)
                return Forbid("Only allowed for admin privileged user!");

            User duplicateUser = _dataAccessProvider.UserRepository.GetByCondition(u => 
                u.Id != updatedUser.Id 
                && (u.UserName == updatedUser.UserName || u.Email == updatedUser.Email)
            ).FirstOrDefault();
            if (duplicateUser != null)
                return BadRequest("A user with same email and/or password exists!");

            updatedUser.UserName = updatedUser.UserName.ToLower();
            updatedUser.Email = updatedUser.Email.ToLower();

            User userToUpdate = _dataAccessProvider.UserRepository.Get(updatedUser.Id);
            if (userToUpdate == null)
                return BadRequest("Could not find user to update!");

            if(userToUpdate.Privilege == Privilege.Admin && user.Id != userToUpdate.Id)
                return Forbid("Admin privileged users can only be updated by themselves!");

            string hashedPassword = _hashingHelper.SaltHash(updatedUser.Password);
            updatedUser.Password = hashedPassword;

            _dataAccessProvider.UserRepository.Update(updatedUser.SetUser(userToUpdate));
            _dataAccessProvider.Save();

            return Ok(userToUpdate.Id);
        }

        [HttpDelete]
        public IActionResult Delete([FromHeader(Name = "Authorization")] string token, [FromBody] Guid id)
        {
            if (!_jwtHelper.IsJwtValid(token, true, out Guid userId))
                return Unauthorized();

            User user = _dataAccessProvider.UserRepository.Get(userId);
            if (user.Privilege != Privilege.Admin)
                return Forbid("Only allowed for admin privileged user!");

            User userToDelete = _dataAccessProvider.UserRepository.Get(id);
            if (userToDelete == null)
                return BadRequest("Could not find user to delete!");

            if (userToDelete.Privilege == Privilege.Admin && userToDelete.Id != userId)
                return Forbid("Admin privileged users can only be deleted by themselves!");

            _dataAccessProvider.UserRepository.Delete(new User { Id = id });
            _dataAccessProvider.Save();

            return Ok(id);
        }
    }
}
