/**
 * Interface for publishing domain events to interested subscribers.
 * <p>
 * The publisher is the entry point for events in the system. It
 * decouples event producers from event consumers. Implementations
 * may use an in-process bus, a message queue, or a streaming platform.
 */
package com.example.events;

public interface EventPublisher {

    /**
     * Publish an event to all registered subscribers.
     *
     * @param event the event object to publish
     * @param <T>   the concrete event type
     */
    <T> void publish(T event);

    /**
     * Register a subscriber to receive events of the given type.
     *
     * @param eventType  the class of events to subscribe to
     * @param subscriber the subscriber that will handle the events
     * @param <T>        the concrete event type
     */
    <T> void subscribe(Class<T> eventType, EventSubscriber<T> subscriber);
}
