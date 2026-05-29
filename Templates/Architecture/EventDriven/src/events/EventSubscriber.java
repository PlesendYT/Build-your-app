/**
 * Functional interface for subscribers that handle events.
 * <p>
 * Subscribers contain the logic that runs in response to an event.
 * Each subscriber should focus on a single concern (e.g., sending
 * an email, updating inventory, firing a notification).
 */
package com.example.events;

/**
 * A subscriber that processes events of type T.
 *
 * @param <T> the event type this subscriber can handle
 */
@FunctionalInterface
public interface EventSubscriber<T> {

    /**
     * Handle the given event. Implementations should be idempotent
     * and resilient to failures.
     *
     * @param event the event to process
     */
    void onEvent(T event);
}
