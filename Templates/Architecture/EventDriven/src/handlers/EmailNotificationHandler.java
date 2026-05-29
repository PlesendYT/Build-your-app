/**
 * Event handler that sends an email notification when an order is created.
 * <p>
 * This handler subscribes to OrderCreatedEvent and sends a confirmation
 * email to the customer. In a real system it would integrate with an
 * email service provider (SendGrid, AWS SES, etc.).
 */
package com.example.handlers;

import com.example.events.EventSubscriber;
import com.example.events.OrderCreatedEvent;

public class EmailNotificationHandler implements EventSubscriber<OrderCreatedEvent> {

    /**
     * Send an order confirmation email.
     * Logs the notification to stdout as a stand-in for a real email client.
     */
    @Override
    public void onEvent(OrderCreatedEvent event) {
        String message = String.format(
            "Email sent to customer %s: Order %s confirmed. Total: $%.2f",
            event.getCustomerId(), event.getOrderId(), event.getTotal()
        );
        System.out.println("[EMAIL] " + message);
        // Integration with an email service would happen here
    }
}
