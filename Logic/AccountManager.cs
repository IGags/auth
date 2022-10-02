using System;
using DAL;
using DAL.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Logic
{
    public class AccountManager
    {
        private readonly IUserRepository _repository;
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountManager(IUserRepository repository, IHttpContextAccessor accessor)
        {
            _repository = repository;
            _contextAccessor = accessor;
        }

        public async Task<bool> IsUserExistAsync(UserDal user)
        {
            return await _repository.IsUserExistsAsync(user);
        }

        public void AddUser(UserDal user)
        {
            _repository.CreateAsync(user);
        }

        public async Task<UserDal> GetByPhoneAsync(string phone)
        {
            return await _repository.GetByPhoneAsync(phone);
        }

        public async Task<bool> LogInUserAsync(UserDal logInData)
        {
            var user = await GetByPhoneAsync(logInData.Phone);
            if (user == null || user.Password != logInData.Password) return false;
            var lastLogin = DateTime.Now;
            user.LastLogin = lastLogin;
            await _repository.UpdateLastLoginAsync(user.Phone, lastLogin);
            await Authenticate(user);
            return true;
        }

        public async Task Authenticate(UserDal user)
        {
            var claims = new List<Claim>
            {
                new ("Phone", user.Phone),
                new ("FIO", user.FIO),
                new ("Email", user.Email),
                new ("LastLogin", user.LastLogin.ToString())
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task LogoutAsync()
        {
            await _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}