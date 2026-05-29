/**
 * Domain model for an Order used in both command and query sides.
 * <p>
 * In a full CQRS implementation the write model and read model may
 * differ. Here we use a single domain model for simplicity, with
 * the understanding that in production the read side would use
 * denormalized projections optimized for query performance.
 */
package com.example.domain;

import java.time.Instant;
import java.util.List;
import java.util.UUID;

public class Order {

    public enum Status { PENDING, CONFIRMED, SHIPPED, DELIVERED, CANCELLED }

    private final String id;
    private final String customerId;
    private final List<OrderItem> items;
    private final double total;
    private Status status;
    private final Instant createdAt;
    private Instant updatedAt;

    /** A single product line item within the order. */
    public record OrderItem(String productId, int quantity, double price) {}

    public Order(String customerId, List<OrderItem> items) {
        this.id = UUID.randomUUID().toString();
        this.customerId = customerId;
        this.items = List.copyOf(items);
        this.total = items.stream().mapToDouble(i -> i.price() * i.quantity()).sum();
        this.status = Status.PENDING;
        this.createdAt = Instant.now();
        this.updatedAt = this.createdAt;
    }

    public String getId() { return id; }
    public String getCustomerId() { return customerId; }
    public List<OrderItem> getItems() { return items; }
    public double getTotal() { return total; }
    public Status getStatus() { return status; }
    public void setStatus(Status status) { this.status = status; this.updatedAt = Instant.now(); }
    public Instant getCreatedAt() { return createdAt; }
    public Instant getUpdatedAt() { return updatedAt; }
}
