using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Areas.Api.Models;
using Api.Models;
using Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Areas.Api.Controllers
{
    [Area("Api"), Route("api/account"), ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly AccountManager _accountManager;
        public AccountApiController(AccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();
            var userDal = request.RegisterDtoToUserDal();
            if (await _accountManager.IsUserExistAsync(userDal))
                return BadRequest(new ExceptionResponse
                    { Message = "Пользователь уже существует", StatusCode = "400" });
            _accountManager.AddUser(userDal);
            return Ok();

        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody]LoginRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();
            var userDal = request.AuthDtoToUserDal();
            if (await _accountManager.LogInUserAsync(userDal)) return Ok();
            return BadRequest(new ExceptionResponse{Message = "Неверные данные авторизации", StatusCode = "400"});

        }

        [HttpPost("logout"), Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _accountManager.LogoutAsync();
            return Ok();
        }

        [HttpGet("get-my-info"), Authorize]
        public IActionResult GetUserInfo()
        {
            var userResponse = User.Claims.ClaimsToResponse();
            return Ok(userResponse);
        }
    }
}
