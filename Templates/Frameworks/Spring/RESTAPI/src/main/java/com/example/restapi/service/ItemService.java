/**
 * Service layer that delegates CRUD operations to the ItemRepository.
 */
package com.example.restapi.service;

import com.example.restapi.model.Item;
import com.example.restapi.repository.ItemRepository;
import org.springframework.stereotype.Service;
import java.util.List;

/** Business-logic facade for Item entities. */
@Service
public class ItemService {
    private final ItemRepository repository;

    public ItemService(ItemRepository repository) {
        this.repository = repository;
    }

    /** Returns all items. */
    public List<Item> findAll() {
        return repository.findAll();
    }

    /** Looks up an item by ID; throws if not found. */
    public Item findById(Long id) {
        return repository.findById(id).orElseThrow();
    }

    /** Persists a new or existing item. */
    public Item save(Item item) {
        return repository.save(item);
    }

    /** Deletes an item by ID. */
    public void delete(Long id) {
        repository.deleteById(id);
    }
}
