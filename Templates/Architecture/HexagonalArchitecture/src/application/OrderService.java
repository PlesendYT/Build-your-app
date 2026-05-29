/**
 * Application service for order management.
 * <p>
 * This is the "use case" layer: it orchestrates domain logic by
 * calling domain model methods and port interfaces. It depends only
 * on abstractions (ports), never on concrete adapters.
 */
package com.example.application;

import com.example.domain.model.Order;
import com.example.domain.model.OrderItem;
import com.example.domain.ports.OrderRepository;
import java.util.List;
import java.util.Optional;

public class OrderService {

    private final OrderRepository orderRepository;

    public OrderService(OrderRepository orderRepository) {
        this.orderRepository = orderRepository;
    }

    /**
     * Place a new order for a customer with the given items.
     *
     * @param customerId the identifier of the customer placing the order
     * @param items      the line items included in the order
     * @return the newly created and persisted Order
     */
    public Order placeOrder(String customerId, List<OrderItem> items) {
        Order order = new Order(customerId, items);
        // Additional business logic (validation, pricing, etc.) goes here
        return orderRepository.save(order);
    }

    /** Retrieve an order by its ID. */
    public Optional<Order> getOrder(String id) {
        return orderRepository.findById(id);
    }

    /** List all orders in the system. */
    public List<Order> listOrders() {
        return orderRepository.findAll();
    }

    /**
     * Cancel an order if it is still in PENDING status.
     *
     * @param id the identifier of the order to cancel
     * @return true if the order was cancelled, false otherwise
     */
    public boolean cancelOrder(String id) {
        Optional<Order> result = orderRepository.findById(id);
        if (result.isPresent()) {
            Order order = result.get();
            if (order.getStatus() == Order.Status.PENDING) {
                order.setStatus(Order.Status.CANCELLED);
                orderRepository.save(order);
                return true;
            }
        }
        return false;
    }
}
