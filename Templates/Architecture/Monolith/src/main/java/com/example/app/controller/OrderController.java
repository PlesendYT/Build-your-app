/**
 * REST controller for order management within the monolith.
 * <p>
 * Because this is a monolith, OrderController can directly access
 * the User module's classes in the same JVM without HTTP calls.
 */
package com.example.app.controller;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

@RestController
@RequestMapping("/api/orders")
public class OrderController {

    private final Map<String, Map<String, Object>> orders = new ConcurrentHashMap<>();

    /**
     * Place a new order.
     *
     * @param body the request payload with "customerId" and "items"
     * @return the created order
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
     * Retrieve an order by ID.
     *
     * @param id the order's identifier
     * @return the order data or 404
     */
    @GetMapping("/{id}")
    public ResponseEntity<Map<String, Object>> getOrder(@PathVariable String id) {
        Map<String, Object> order = orders.get(id);
        if (order == null) return ResponseEntity.notFound().build();
        return ResponseEntity.ok(order);
    }

    /** List all orders. */
    @GetMapping
    public ResponseEntity<?> listOrders() {
        return ResponseEntity.ok(orders.values());
    }
}
