// File: Program.cs
// ASP.NET Core Minimal API for a RESTful item CRUD service. Includes Swagger, CORS, and in-memory storage.

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();

// In-memory item store
var items = new List<Item>();

// GET all items
app.MapGet("/api/items", () => items);
// GET a single item by ID
app.MapGet("/api/items/{id}", (int id) =>
    items.FirstOrDefault(i => i.Id == id) is { } item ? Results.Ok(item) : Results.NotFound());
// POST a new item
app.MapPost("/api/items", (Item item) =>
{
    item.Id = items.Count + 1;
    items.Add(item);
    return Results.Created($"/api/items/{item.Id}", item);
});
// DELETE an item by ID
app.MapDelete("/api/items/{id}", (int id) =>
{
    items.RemoveAll(i => i.Id == id);
    return Results.NoContent();
});

app.Run();

/// <summary>
/// Represents an item in the REST API with an auto-generated ID, title, and description.
/// </summary>
record Item(int Id, string Title, string Description);
