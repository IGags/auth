using System.Threading.Tasks;
using Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    public class CabinetController : Controller
    {
        private readonly AccountManager _accountManager;
        public CabinetController(AccountManager manager)
        {
            _accountManager = manager;
        }

		[Route("cabinet")]
		public IActionResult GetInfo()
		{
            return View(User.Claims);
        }

        [Route("[action]")]
        public async Task<IActionResult> LogOut()
        {
            await _accountManager.LogoutAsync();
            return RedirectToAction("Login", "Account");
        }
	}
}
