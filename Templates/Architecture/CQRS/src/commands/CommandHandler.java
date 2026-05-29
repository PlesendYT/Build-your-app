/**
 * Handler interface for processing commands in the CQRS pipeline.
 * <p>
 * Each command type has a dedicated handler that encapsulates the
 * write-side business logic. Handlers are invoked by a command bus
 * or dispatcher.
 */
package com.example.commands;

/**
 * Generic command handler interface.
 *
 * @param <C> the command type this handler processes
 * @param <R> the result type returned after command execution
 */
public interface CommandHandler<C, R> {

    /**
     * Execute the given command and return a result.
     *
     * @param command the command to process
     * @return the execution result
     */
    R handle(C command);
}
