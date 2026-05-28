using Microsoft.AspNetCore.Mvc;

namespace RESTAPIASP.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private static readonly List<Item> Items = [];
    private static int _nextId = 1;

    [HttpGet]
    public ActionResult<IEnumerable<Item>> GetAll()
    {
        return Ok(Items);
    }

    [HttpGet("{id}")]
    public ActionResult<Item> GetById(int id)
    {
        var item = Items.FirstOrDefault(i => i.Id == id);
        return item is not null ? Ok(item) : NotFound();
    }

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

    [HttpPut("{id}")]
    public ActionResult Update(int id, ItemCreateDto dto)
    {
        var index = Items.FindIndex(i => i.Id == id);
        if (index < 0) return NotFound();

        Items[index] = Items[index] with
        {
            Name = dto.Name,
            Value = dto.Value
        };
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var removed = Items.RemoveAll(i => i.Id == id);
        return removed > 0 ? NoContent() : NotFound();
    }

    [HttpGet("health")]
    public ActionResult Health()
    {
        return Ok(new { status = "ok", version = "1.0.0" });
    }
}

public record Item
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}

public record ItemCreateDto
{
    public string Name { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
}
