using System.Threading.Tasks;
using Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[Controller]
public class CabinetController : Controller
{
    private readonly AccountManager _accountManager;

    public CabinetController(AccountManager manager)
    {
        _accountManager = manager;
    }

    [HttpGet("cabinet")]
    public IActionResult GetInfo()
    {
        return View(User.Claims);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogOut()
    {
        await _accountManager.LogoutAsync();
        return RedirectToAction("Login", "Account");
    }
}