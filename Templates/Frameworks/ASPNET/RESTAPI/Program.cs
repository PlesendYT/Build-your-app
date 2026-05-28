var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();

var items = new List<Item>();

app.MapGet("/api/items", () => items);
app.MapGet("/api/items/{id}", (int id) =>
    items.FirstOrDefault(i => i.Id == id) is { } item ? Results.Ok(item) : Results.NotFound());
app.MapPost("/api/items", (Item item) =>
{
    item.Id = items.Count + 1;
    items.Add(item);
    return Results.Created($"/api/items/{item.Id}", item);
});
app.MapDelete("/api/items/{id}", (int id) =>
{
    items.RemoveAll(i => i.Id == id);
    return Results.NoContent();
});

app.Run();

record Item(int Id, string Title, string Description);
