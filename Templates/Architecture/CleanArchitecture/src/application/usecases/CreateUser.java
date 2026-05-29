/**
 * Application use case: Create a new user.
 * <p>
 * Orchestrates the flow from input (request) through business rules to
 * output (response). Depends on the UserRepository interface, not on
 * any specific persistence mechanism.
 */
package com.example.application.usecases;

import com.example.domain.entities.User;
import com.example.domain.repositories.UserRepository;

public class CreateUser {

    private final UserRepository userRepository;

    public CreateUser(UserRepository userRepository) {
        this.userRepository = userRepository;
    }

    /**
     * Input data required to create a new user.
     *
     * @param name  the display name for the new user
     * @param email the email address for the new user
     */
    public record CreateUserRequest(String name, String email) {}

    /**
     * Output data returned after successful user creation.
     *
     * @param id    the generated user identifier
     * @param name  the display name of the created user
     * @param email the email address of the created user
     */
    public record CreateUserResponse(String id, String name, String email) {}

    /**
     * Execute the "create user" use case.
     *
     * @param request the input data for the new user
     * @return the response containing the created user's details
     * @throws IllegalArgumentException if the email is already in use
     */
    public CreateUserResponse execute(CreateUserRequest request) {
        if (userRepository.existsByEmail(request.email())) {
            throw new IllegalArgumentException("Email already in use: " + request.email());
        }
        User user = new User(request.name(), request.email());
        User saved = userRepository.save(user);
        return new CreateUserResponse(saved.getId(), saved.getName(), saved.getEmail());
    }
}
