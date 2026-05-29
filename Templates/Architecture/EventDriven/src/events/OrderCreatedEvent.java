/**
 * Event representing that an order has been created in the system.
 * <p>
 * In an event-driven architecture, events are immutable records of
 * something that happened. They are produced by event publishers and
 * consumed by one or more event subscribers.
 */
package com.example.events;

import java.time.Instant;
import java.util.UUID;

public class OrderCreatedEvent {

    private final String eventId;
    private final String orderId;
    private final String customerId;
    private final double total;
    private final Instant occurredAt;

    /**
     * @param orderId    the unique identifier of the created order
     * @param customerId the customer who placed the order
     * @param total      the total amount of the order
     */
    public OrderCreatedEvent(String orderId, String customerId, double total) {
        this.eventId = UUID.randomUUID().toString();
        this.orderId = orderId;
        this.customerId = customerId;
        this.total = total;
        this.occurredAt = Instant.now();
    }

    public String getEventId() { return eventId; }
    public String getOrderId() { return orderId; }
    public String getCustomerId() { return customerId; }
    public double getTotal() { return total; }
    public Instant getOccurredAt() { return occurredAt; }
}
