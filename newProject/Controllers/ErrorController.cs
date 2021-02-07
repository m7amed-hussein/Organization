using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace newProject.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> ilogger;

        public ErrorController(ILogger<ErrorController> ilogger)
        {
            this.ilogger = ilogger;
        }
        // GET: /<controller>/
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resourse not found";
                    ilogger.LogWarning($"404 error occured with path ={statusCodeResult.OriginalPath}" +
                        $" and Querying string ={statusCodeResult.OriginalQueryString}");
                    break;
                default:
                    ViewBag.ErrorMessage = "Sorry, any other error not a 404";
                    break;
                       
            }
            return View("NotFound");
        }
        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ilogger.LogError($"path = {exceptionDetails.Path} threw an exception " +
                $"{exceptionDetails.Error}") ;
            return View("Error");
        }
    }
}
