using Microsoft.AspNetCore.Mvc;
using PlumbworldDemo.Models.Products;
using PlumbworldDemo.Services;

namespace PlumbworldDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
