using Microsoft.AspNetCore.Mvc;

namespace CarRent.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
