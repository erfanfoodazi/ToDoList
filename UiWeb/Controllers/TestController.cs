using Microsoft.AspNetCore.Mvc;

namespace UiWeb.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return  Content("Hello World! The application is working.");
        }
    }
}