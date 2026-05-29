/**
 * Persistence adapter implementing the OrderRepository port.
 * <p>
 * This adapter sits in the "driven" side of the hexagon and translates
 * between the domain model and the actual storage mechanism (in-memory
 * map in this example). The application layer knows nothing about this
 * implementation—it only depends on the port interface.
 */
package com.example.adapter.persistence;

import com.example.domain.model.Order;
import com.example.domain.ports.OrderRepository;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;

public class OrderRepositoryImpl implements OrderRepository {

    private final Map<String, Order> store = new HashMap<>();

    @Override
    public Order save(Order order) {
        store.put(order.getId(), order);
        return order;
    }

    @Override
    public Optional<Order> findById(String id) {
        return Optional.ofNullable(store.get(id));
    }

    @Override
    public List<Order> findByCustomerId(String customerId) {
        return store.values().stream()
                .filter(o -> o.getCustomerId().equals(customerId))
                .toList();
    }

    @Override
    public List<Order> findAll() {
        return new ArrayList<>(store.values());
    }

    @Override
    public void deleteById(String id) {
        store.remove(id);
    }
}
