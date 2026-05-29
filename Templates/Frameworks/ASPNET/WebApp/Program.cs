// File: Program.cs
// ASP.NET Core Web App (Razor Pages) entry point. Enables Razor Pages and maps their routes.

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
var app = builder.Build();
app.MapRazorPages();
app.Run();
