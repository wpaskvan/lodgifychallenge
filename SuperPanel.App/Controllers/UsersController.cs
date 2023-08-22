using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuperPanel.App.Managers.Interfaces;
using SuperPanel.App.Models;
using System;
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
            try
            {
                var result = await _userManager.GetUsersByPageAsync(page, pageSize);

                return View(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred processing the request: {error}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                _logger.LogTrace("Action Users:Index finished");
            }
        }

        public async Task<IActionResult> Details(int userId)
        {
            _logger.LogTrace("Action Users:Details started");
            try
            {
                var result = await _userManager.GetUserByIdAsync(userId);
                if(result == null)
                {
                    return NotFound();
                }

                return View(result);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex, "An error occurred processing the request: {error}", ex.Message);
                return BadRequest();
            }catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred processing the request: {error}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                _logger.LogTrace("Action Users:Details finished");
            }
        }
    }
}
