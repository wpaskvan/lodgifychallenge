using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuperApp.Core.Exceptions;
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

        [Route("{controller}/delete/{userId}")]
        public async Task<IActionResult> Delete([FromRoute] int userId)
        {
            _logger.LogTrace("Action Users:Delete started");
            try
            {
                await _userManager.GdprDeleteAsync(userId);
                return View();
            }catch(NotFoundException ex)
            {
                _logger.LogError(ex, "An error occurred processing the request: Resource not found {resourceId}", ex.ResourceId);
                return NotFound();
            }catch(ExternalApiException ex)
            { 
                _logger.LogError(ex, "An error occurred processing the request: External API responds with {statusCode} {response}", ex.StatusCode, ex.ResponseMessage);
                return StatusCode(ex.StatusCode);
            }
            finally
            {
                _logger.LogTrace("Action Users:Delete finished");
            }
        }
    }
}
