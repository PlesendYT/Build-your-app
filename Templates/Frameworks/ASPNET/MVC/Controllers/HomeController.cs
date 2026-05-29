// File: HomeController.cs
// Home controller for the ASP.NET Core MVC template. Serves Index and About pages.

using Microsoft.AspNetCore.Mvc;

namespace MvcApp.Controllers;

/// <summary>
/// Handles requests for the home section of the MVC app. Returns default views for Index and About actions.
/// </summary>
public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult About() => View();
}
