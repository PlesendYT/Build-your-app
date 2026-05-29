using Microsoft.AspNetCore.Mvc;

namespace RESTAPIASP.Controllers;

/// <summary>REST API controller for CRUD operations on items.</summary>
[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    // In-memory item store (static for demo purposes)
    private static readonly List<Item> Items = [];
    private static int _nextId = 1;

    /// <summary>Returns all items.</summary>
    [HttpGet]
    public ActionResult<IEnumerable<Item>> GetAll()
    {
        return Ok(Items);
    }

    /// <summary>Returns a single item by ID, or 404 if not found.</summary>
    [HttpGet("{id}")]
    public ActionResult<Item> GetById(int id)
    {
        var item = Items.FirstOrDefault(i => i.Id == id);
        return item is not null ? Ok(item) : NotFound();
    }

    /// <summary>Creates a new item and returns it with a 201 Created response.</summary>
    [HttpPost]
    public ActionResult<Item> Create(ItemCreateDto dto)
    {
        var item = new Item
        {
            Id = _nextId++,
            Name = dto.Name,
            Value = dto.Value,
            CreatedAt = DateTime.UtcNow
        };
        Items.Add(item);
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    /// <summary>Updates an existing item by ID, or returns 404 if not found.</summary>
    [HttpPut("{id}")]
    public ActionResult Update(int id, ItemCreateDto dto)
    {
        var index = Items.FindIndex(i => i.Id == id);
        if (index < 0) return NotFound();

        // Use non-destructive mutation via "with" expression
        Items[index] = Items[index] with
        {
            Name = dto.Name,
            Value = dto.Value
        };
        return NoContent();
    }

    /// <summary>Deletes an item by ID, or returns 404 if not found.</summary>
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var removed = Items.RemoveAll(i => i.Id == id);
        return removed > 0 ? NoContent() : NotFound();
    }

    /// <summary>Simple health-check endpoint.</summary>
    [HttpGet("health")]
    public ActionResult Health()
    {
        return Ok(new { status = "ok", version = "1.0.0" });
    }
}

/// <summary>Represents an item with ID, name, value, and creation timestamp.</summary>
public record Item
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}

/// <summary>DTO used when creating or updating an item.</summary>
public record ItemCreateDto
{
    public string Name { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
}
