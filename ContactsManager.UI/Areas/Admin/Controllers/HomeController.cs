using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //for role based authentication:
    [Authorize(Roles ="Admin")] // means this admin page is authorized to Admin users only.
    //for role based authentication:
    public class HomeController : Controller
    {
        //[Route("admin/home/index")] // if we want to use conventional routing then dont use this. //eg: Admin/Home/Index
        //[Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
