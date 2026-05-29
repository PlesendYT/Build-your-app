/**
 * Web adapter exposing REST endpoints for order operations.
 * <p>
 * This adapter sits on the "driving" side of the hexagon: it receives
 * HTTP requests and translates them into calls on the application
 * service port. It is a thin, framework-aware layer that should
 * contain zero business logic.
 */
package com.example.adapter.web;

import com.example.application.OrderService;
import com.example.domain.model.Order;
import com.example.domain.model.OrderItem;
import java.util.List;

/**
 * REST controller for order endpoints.
 * In a production system this would use @RestController annotations
 * from Spring or equivalent. Here we keep it framework-agnostic.
 */
public class OrderController {

    private final OrderService orderService;

    public OrderController(OrderService orderService) {
        this.orderService = orderService;
    }

    /** POST /orders — create a new order. */
    public Order createOrder(String customerId, List<OrderItem> items) {
        return orderService.placeOrder(customerId, items);
    }

    /** GET /orders/{id} — retrieve a single order. */
    public Order getOrder(String id) {
        return orderService.getOrder(id)
                .orElseThrow(() -> new RuntimeException("Order not found: " + id));
    }

    /** GET /orders — list all orders. */
    public List<Order> listOrders() {
        return orderService.listOrders();
    }

    /** DELETE /orders/{id} — cancel an order. */
    public boolean cancelOrder(String id) {
        return orderService.cancelOrder(id);
    }
}
