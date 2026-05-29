/**
 * Controller that handles HTTP requests for user operations.
 * <p>
 * Interface layer: adapts external input (HTTP requests) to the
 * application use cases. This controller is intentionally decoupled
 * from any specific web framework so it can be reused across
 * different delivery mechanisms (REST, GraphQL, CLI, etc.).
 */
package com.example.interfaces.controllers;

import com.example.application.usecases.CreateUser;
import com.example.application.usecases.CreateUser.CreateUserRequest;
import com.example.application.usecases.CreateUser.CreateUserResponse;

public class UserController {

    private final CreateUser createUser;

    public UserController(CreateUser createUser) {
        this.createUser = createUser;
    }

    /**
     * Handle a "create user" request. This would typically be invoked
     * by a framework-specific adapter (e.g., a Spring @RestController).
     *
     * @param name  the display name from the HTTP request body
     * @param email the email address from the HTTP request body
     * @return the response DTO with the created user's details
     */
    public CreateUserResponse handleCreateUser(String name, String email) {
        return createUser.execute(new CreateUserRequest(name, email));
    }
}
