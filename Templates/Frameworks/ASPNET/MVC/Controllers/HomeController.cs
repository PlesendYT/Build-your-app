using Microsoft.AspNetCore.Mvc;

namespace MvcApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult About() => View();
}
