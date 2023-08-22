using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SuperPanel.App.Models;
using System;

namespace SuperPanel.App.Controllers
{
    public class ErrorsController : Controller
    {
        [Route("Errors/{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            ErrorViewModel model;
            switch (statusCode)
            {
                case 400:
                    model = BuilModel(statusCode, "Invalid request.Verify the data and try again", "Bad Request");
                    break;
                case 404:
                    model = BuilModel(statusCode, "The resource that is trying to access does not exists", "Not Found");
                    break;
                case 500:
                    model = BuilModel(statusCode, "We are facing issues to process your request. Try again later", "Internal Server Error");
                    break;
                default:
                    model = BuilModel(statusCode, "We are facing issues to process your request. Try again later", "Internal Server Error");
                    break;
            }
            return View("Error", model);
        }

        private static ErrorViewModel BuilModel(int statusCode, string errorMessage, string title)
        {
            return new ErrorViewModel()
            {
                StatusCode = statusCode,
                Error = errorMessage,
                Title = title
            };
        }
    }
}
