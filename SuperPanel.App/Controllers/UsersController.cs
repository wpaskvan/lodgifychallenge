using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuperPanel.App.Managers.Interfaces;
using SuperPanel.App.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SuperPanel.App.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserManager _userManager;

        public UsersController(ILogger<UsersController> logger, IUserManager userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 25)
        {
            _logger.LogTrace("Action Users:Index started");
            var result = await _userManager.GetUsersByPageAsync(page, pageSize);

            _logger.LogTrace("Action Users:Index finished");
            return View(result);
        }
    }
}
