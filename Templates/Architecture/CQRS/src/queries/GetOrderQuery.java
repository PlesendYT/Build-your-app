/**
 * Query object representing a request to retrieve an order by ID.
 * <p>
 * In CQRS, queries are read-side operations that return data without
 * mutating state. They are separated from commands to allow independent
 * scaling and optimization of read and write paths.
 */
package com.example.queries;

import java.util.Objects;

public class GetOrderQuery {

    private final String orderId;

    /**
     * @param orderId the unique identifier of the order to retrieve
     */
    public GetOrderQuery(String orderId) {
        this.orderId = Objects.requireNonNull(orderId, "orderId must not be null");
    }

    public String getOrderId() { return orderId; }
}
