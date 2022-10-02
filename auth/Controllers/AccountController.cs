using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Models;
using DAL.Models;
using Logic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Api.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly AccountManager _accountManager;
        public AccountController(AccountManager manager)
        {
            _accountManager = manager;
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (ModelState.IsValid)
            {
                var userDal = request.RegisterDtoToUserDal();
                if (!await _accountManager.IsUserExistAsync(userDal))
                {
                    _accountManager.AddUser(userDal);
                    return RedirectToAction("RegisterSuccess", "Account", new{ userDal.FIO });
                }
                ModelState.AddModelError(String.Empty, $"Account with {userDal.Email} or {userDal.Phone} has already exists");
            }

            return View(request);
        }

        [HttpGet("register-success")]
        public IActionResult RegisterSuccess(string fio)
        {
            ViewBag.FIO = fio;
            return View();
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (ModelState.IsValid)
            {
                if (await _accountManager.LogInUserAsync(request.AuthDtoToUserDal()))
                {
                    return RedirectToAction("GetInfo", "Cabinet");
                }
            }

            ModelState.AddModelError(String.Empty, "Ошибка авторизации");
            return View(request);
        }


    }
}
