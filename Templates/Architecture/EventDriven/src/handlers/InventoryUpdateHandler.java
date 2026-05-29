/**
 * Event handler that updates inventory levels when an order is created.
 * <p>
 * This handler subscribes to OrderCreatedEvent and decreases the
 * available stock quantity for each product in the order. In production
 * it would query an inventory database or service.
 */
package com.example.handlers;

import com.example.events.EventSubscriber;
import com.example.events.OrderCreatedEvent;

public class InventoryUpdateHandler implements EventSubscriber<OrderCreatedEvent> {

    /**
     * Decrease inventory for each item in the order.
     * Currently logs the deduction as a stand-in for a real inventory system.
     */
    @Override
    public void onEvent(OrderCreatedEvent event) {
        System.out.printf(
            "[INVENTORY] Order %s: inventory updated for customer %s%n",
            event.getOrderId(), event.getCustomerId()
        );
        // In a real system this would call inventoryService.deductStock(...)
    }
}
