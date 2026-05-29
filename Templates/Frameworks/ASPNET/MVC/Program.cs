// File: Program.cs
// ASP.NET Core MVC app entry point. Configures services and maps the default controller route.

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.Run();
