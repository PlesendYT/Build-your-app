/**
 * Spring REST controller exposing CRUD endpoints for Item entities.
 */
package com.example.restapi.controller;

import com.example.restapi.model.Item;
import com.example.restapi.service.ItemService;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;
import jakarta.validation.Valid;
import java.util.List;

/** Handles HTTP requests for the /api/items resource. */
@RestController
@RequestMapping("/api/items")
public class ItemController {
    private final ItemService service;

    public ItemController(ItemService service) {
        this.service = service;
    }

    /** Returns all items. */
    @GetMapping
    public List<Item> getAll() {
        return service.findAll();
    }

    /** Returns a single item by ID. */
    @GetMapping("/{id}")
    public Item getById(@PathVariable Long id) {
        return service.findById(id);
    }

    /** Creates a new item and responds with HTTP 201. */
    @PostMapping
    @ResponseStatus(HttpStatus.CREATED)
    public Item create(@Valid @RequestBody Item item) {
        return service.save(item);
    }

    /** Deletes an item by ID and responds with HTTP 204. */
    @DeleteMapping("/{id}")
    @ResponseStatus(HttpStatus.NO_CONTENT)
    public void delete(@PathVariable Long id) {
        service.delete(id);
    }
}
