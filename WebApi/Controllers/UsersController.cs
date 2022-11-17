using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtAuthentication.Server.Data;
using JwtAuthentication.Server.Models;
using JwtAuthentication.Server.Services;
using Microsoft.AspNetCore.Authorization;
using JwtAuthentication.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JwtAuthentication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAccountServices _accountServices;
        public UsersController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }


        [Route("register")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] SignUpDto user)
        {
            var createduser = await _accountServices.CreateUserAsync(user);
            if (createduser.Succeeded)
            {
                return Ok(new { createduser.Succeeded  });
            }else
            {
                string errorToReturn = "Register Failed with the following error";
                foreach (var error in createduser.Errors)
                {
                    errorToReturn +=Environment.NewLine;
                    errorToReturn += $"Error Code : {error.Code} - {error.Description} ";
                }
                return StatusCode(StatusCodes.Status500InternalServerError,errorToReturn);
            }
        }
        [Route("signin")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInDto user)
        {
            var signinuser = await _accountServices.PasswordSignInAsync(user);
            if (signinuser.Succeeded)
            {
                User userResult = await _accountServices.GetUserByEmailAsync(user.Email);
                string token = await _accountServices.GenerateJSONWebTocken(userResult);
                return Ok(token);
            }else
            {
                return Unauthorized(signinuser);
            }
        }
    }
}

//// GET: api/UsersController
//[Route("GetAllClients")]
//[HttpGet]
//public async Task<IEnumerable<User>> GetAsync()
//{
//    var allusersSerialize = await _accountServices.GetAllClient();
//    var jsonString = JsonConvert.SerializeObject(allusersSerialize, Formatting.None, new JsonSerializerSettings()
//    {
//        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
//    });
//    List<User> allusers = JsonConvert.DeserializeObject<List<User>>(jsonString);
//    return allusers;
//}

//// GET api/UsersController/5
//[HttpGet("{id}")]
//public async Task<User> GetAsync(string id)
//{
//    var user = await _accountServices.GetUserByIdAsync(id);
//    return user;
//}
//// GET api/UsersController/5
//[HttpGet("CodeClient/{id}")]
//public async Task<User> GetWithCodeClAsync(string id)
//{
//    var user = await _accountServices.GetUserByUserNameAsync(id);
//    return user;
//}

//// GET api/UsersController/5
//[HttpGet("VerifAsync/{id}")]
//public async Task<bool> VerifAsync(string id)
//{
//    var user = await _accountServices.GetUserByUserNameAsync(id);
//    if (user != null)
//    {
//        return true;
//    }
//    return false;
//}
//// POST api/UsersController
//[HttpPost]
//public async Task<string> PostAsync([FromBody] SignUpDto user)
//{
//    var createduser = await _accountServices.CreateUserAsync(user);
//    return createduser.ToString();
//}

//// PUT api/UsersController/5
//[HttpPut("{id}")]
//public async Task PutAsync(string id, [FromBody] SignUpDto user)
//{
//    var createduser = await _accountServices.UpdateAutreUser(id, user);
//}

//// DELETE api/UsersController/5
//[HttpDelete("{id}")]
//public async Task DeleteAsync(string id)
//{
//    await _accountServices.DeleteUserAsync(id);
//}