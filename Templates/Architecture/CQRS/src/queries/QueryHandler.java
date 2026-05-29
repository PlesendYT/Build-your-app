/**
 * Handler interface for processing queries in the CQRS pipeline.
 * <p>
 * Query handlers are responsible for reading data from the read-optimized
 * store. They never modify state and typically have no side effects.
 *
 * @param <Q> the query type this handler processes
 * @param <R> the result type returned (typically a DTO or projection)
 */
package com.example.queries;

public interface QueryHandler<Q, R> {

    /**
     * Execute the given query and return the result.
     *
     * @param query the query to process
     * @return the query result (read model projection)
     */
    R handle(Q query);
}
