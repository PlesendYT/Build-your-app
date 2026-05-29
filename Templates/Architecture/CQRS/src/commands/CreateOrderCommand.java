/**
 * Command object representing the intent to create a new order.
 * <p>
 * In CQRS, commands are write-side operations that mutate state.
 * They are expressed as plain immutable data objects.
 */
package com.example.commands;

import java.util.List;

public class CreateOrderCommand {

    private final String customerId;
    private final List<OrderLineItem> items;

    /** A single line item in the order. */
    public record OrderLineItem(String productId, int quantity, double price) {}

    /**
     * @param customerId the customer placing the order
     * @param items      the products being ordered
     */
    public CreateOrderCommand(String customerId, List<OrderLineItem> items) {
        this.customerId = customerId;
        this.items = List.copyOf(items); // defensive copy
    }

    public String getCustomerId() { return customerId; }
    public List<OrderLineItem> getItems() { return items; }
}
