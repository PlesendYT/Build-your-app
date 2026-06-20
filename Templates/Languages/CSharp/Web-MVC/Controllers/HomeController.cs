using Microsoft.AspNetCore.Mvc;
namespace WebMVC.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index(string? name) => View("Index", name ?? "World");
}