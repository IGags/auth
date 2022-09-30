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

namespace Api.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly AccountManager _accountManager;
        public AccountController(AccountManager manager)
        {
            _accountManager = manager;
        }

        [HttpGet, Route("[action]")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, Route("[action]"), ValidateAntiForgeryToken]
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

        [Route("[action]")]
        public IActionResult RegisterSuccess(string fio)
        {
            ViewBag.FIO = fio;
            return View();
        }

        [Route("[action]"), HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("[action]"), HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AuthenticationRequest request)
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
