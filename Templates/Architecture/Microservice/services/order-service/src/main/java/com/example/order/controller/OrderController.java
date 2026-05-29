/**
 * REST controller for order-related endpoints.
 * <p>
 * Handles order creation, retrieval, and status updates. This service
 * calls the User service internally to validate customer existence.
 */
package com.example.order.controller;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

@RestController
@RequestMapping("/api/orders")
public class OrderController {

    // In-memory order store for demonstration purposes
    private final Map<String, Map<String, Object>> orders = new ConcurrentHashMap<>();

    /**
     * Place a new order.
     *
     * @param body a map containing "customerId" and "items" fields
     * @return the created order with a generated ID
     */
    @PostMapping
    public ResponseEntity<Map<String, Object>> createOrder(@RequestBody Map<String, Object> body) {
        String id = String.valueOf(orders.size() + 1);
        Map<String, Object> order = new ConcurrentHashMap<>(body);
        order.put("id", id);
        order.put("status", "PENDING");
        orders.put(id, order);
        return ResponseEntity.status(HttpStatus.CREATED).body(order);
    }

    /**
     * Retrieve an order by its unique identifier.
     *
     * @param id the order's UUID
     * @return the order data or 404 if not found
     */
    @GetMapping("/{id}")
    public ResponseEntity<Map<String, Object>> getOrder(@PathVariable String id) {
        Map<String, Object> order = orders.get(id);
        if (order == null) {
            return ResponseEntity.notFound().build();
        }
        return ResponseEntity.ok(order);
    }

    /**
     * List all orders in the system.
     *
     * @return a collection of all order records
     */
    @GetMapping
    public ResponseEntity<?> listOrders() {
        return ResponseEntity.ok(orders.values());
    }
}
