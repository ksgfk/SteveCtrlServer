using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SteveCtrl.Models;
using SteveCtrl.Repositories;

namespace SteveCtrl.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController] //ApiController特性用于提供 HTTP API 响应, https://learn.microsoft.com/zh-cn/aspnet/core/web-api/?view=aspnetcore-8.0#apicontroller-attribute
    public class AccountController(ILogger<AccountController> logger, IUserRepository userRepo) : ControllerBase
    {
        private readonly ILogger<AccountController> _logger = logger;
        private readonly IUserRepository _userRepo = userRepo;

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            UserModel? user = _userRepo.GetUserByUsernameAndPassword(model.Username, model.Password);
            if (user == null)
            {
                return Forbid();
            }
            List<Claim> claims =
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role,user.Role.ToString())
            ];
            ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new(identity);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = model.IsRememberMe ?? false });
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetUserInfo()
        {
            var strId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var name = User.FindFirstValue(ClaimTypes.Name);
            var strRole = User.FindFirstValue(ClaimTypes.Role);
            if (strId == null ||
            name == null ||
            strRole == null ||
            !int.TryParse(strId, out int id) ||
            !Enum.TryParse(strRole, out UserRole role))
            {
                return BadRequest();
            }
            UserModel? user = _userRepo.GetUserById(id);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(new { user.Id, user.Name, user.Role });
        }
    }
}
