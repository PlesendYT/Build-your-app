/**
 * Domain model for an Order in the monolithic application.
 * <p>
 * Shares the same database and JPA persistence context as User.
 * Each order is linked to a customer via the customerId field.
 */
package com.example.app.model;

import jakarta.persistence.*;
import java.time.Instant;
import java.util.Objects;

@Entity
@Table(name = "orders")
public class Order {

    public enum Status { PENDING, CONFIRMED, SHIPPED, DELIVERED, CANCELLED }

    @Id
    @GeneratedValue(strategy = GenerationType.UUID)
    private String id;

    @Column(nullable = false)
    private String customerId;

    private double total;

    @Enumerated(EnumType.STRING)
    private Status status;

    private Instant createdAt;
    private Instant updatedAt;

    /** Default constructor required by JPA. */
    protected Order() {}

    public Order(String customerId, double total) {
        this.customerId = customerId;
        this.total = total;
        this.status = Status.PENDING;
        this.createdAt = Instant.now();
        this.updatedAt = this.createdAt;
    }

    public String getId() { return id; }
    public String getCustomerId() { return customerId; }
    public double getTotal() { return total; }
    public Status getStatus() { return status; }
    public void setStatus(Status status) { this.status = status; this.updatedAt = Instant.now(); }
    public Instant getCreatedAt() { return createdAt; }
    public Instant getUpdatedAt() { return updatedAt; }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (!(o instanceof Order order)) return false;
        return Objects.equals(id, order.id);
    }

    @Override
    public int hashCode() { return Objects.hash(id); }
}
