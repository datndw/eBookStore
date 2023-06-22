using Microsoft.AspNetCore.Mvc;

namespace eBookStore.Controllers
{
    public class UnauthorizedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
