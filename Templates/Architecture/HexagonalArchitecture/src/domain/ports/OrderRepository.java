/**
 * Port (output) interface for Order persistence.
 * <p>
 * In Hexagonal Architecture, ports are boundaries through which the
 * application communicates with external systems. This port defines
 * the contract that any persistence adapter must implement.
 */
package com.example.domain.ports;

import com.example.domain.model.Order;
import java.util.List;
import java.util.Optional;

public interface OrderRepository {

    /** Persist a new order or update an existing one. */
    Order save(Order order);

    /** Retrieve an order by its unique identifier. */
    Optional<Order> findById(String id);

    /** Return all orders for a given customer. */
    List<Order> findByCustomerId(String customerId);

    /** Return all orders in the system. */
    List<Order> findAll();

    /** Delete an order by its unique identifier. */
    void deleteById(String id);
}
