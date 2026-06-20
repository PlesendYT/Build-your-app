package example.demo;

import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.concurrent.CopyOnWriteArrayList;
import java.util.concurrent.atomic.AtomicLong;

@RestController
@RequestMapping("/items")
public class ItemController {

    private final List<Item> items = new CopyOnWriteArrayList<>();
    private final AtomicLong nextId = new AtomicLong(1);

    public record Item(long id, String name) {}

    @GetMapping
    public List<Item> all() {
        return items;
    }

    @PostMapping
    public Item create(@RequestBody NewItem body) {
        Item item = new Item(nextId.getAndIncrement(), body.name());
        items.add(item);
        return item;
    }

    @DeleteMapping("/{id}")
    public void delete(@PathVariable long id) {
        items.removeIf(i -> i.id() == id);
    }

    public record NewItem(String name) {}
}