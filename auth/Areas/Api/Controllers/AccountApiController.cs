using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Areas.Api.Models;
using Api.Filters;
using Api.Models;
using Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Areas.Api.Controllers
{
    [Area("Api"), Route("api/account"), ApiController, ApiExceptionFilter]
    public class AccountApiController : ControllerBase
    {
        private readonly AccountManager _accountManager;
        public AccountApiController(AccountManager accountManager, IServiceProvider provider)
        {
            _accountManager = accountManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequest request)
        {
            if (ModelState.IsValid)
            {
                var userDal = request.RegisterDtoToUserDal();
                if (!await _accountManager.IsUserExistAsync(userDal))
                {
                    _accountManager.AddUser(userDal);
                    return Ok();
                }
                return BadRequest(new ExceptionResponse { Message = "Пользователь уже существует", StatusCode = "400" });
            }

            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody]LoginRequest request)
        {
            if (ModelState.IsValid)
            {
                var userDal = request.AuthDtoToUserDal();
                if (await _accountManager.LogInUserAsync(userDal)) return Ok();
                return BadRequest(new ExceptionResponse{Message = "Неверные данные авторизации", StatusCode = "400"});
            }

            return BadRequest();
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
