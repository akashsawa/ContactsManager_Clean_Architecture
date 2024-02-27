using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CrudExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("Error")]
        [AllowAnonymous] // for authentication/authorization : means user without user account.
        public IActionResult Error()
        {
            IExceptionHandlerFeature? exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>(); // for providing addtional detaisl of current errors.

            if(exceptionHandlerFeature != null && exceptionHandlerFeature.Error != null) 
            {
                ViewBag.ErrorMessage = exceptionHandlerFeature.Error.Message;
            }
            else
            {
                ViewBag.ErrorMessage = "Error Ocuured during execution !...";
            }
            return View(); //views/shared/error
        }
    }
}
