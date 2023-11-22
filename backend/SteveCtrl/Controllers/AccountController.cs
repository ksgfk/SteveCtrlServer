using Microsoft.AspNetCore.Mvc;

namespace SteveCtrl
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController(ILogger<AccountController> logger) : Controller
    {
        private readonly ILogger<AccountController> _logger = logger;

        [HttpGet]
        public ActionResult Login()
        {
            _logger.LogInformation("login");
            return Ok("hello?");
        }

        // public ActionResult Logout()
        // {
        //     throw new NotImplementedException();
        // }
    }
}
