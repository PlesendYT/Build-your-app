/**
 * Repository interface for User persistence operations.
 * <p>
 * Defined in the domain layer so that application and infrastructure code
 * depend on this abstraction, not on concrete implementations.
 */
package com.example.domain.repositories;

import com.example.domain.entities.User;
import java.util.List;
import java.util.Optional;

public interface UserRepository {

    /** Persist a new user and return the saved entity with its generated ID. */
    User save(User user);

    /** Find a user by their unique identifier. */
    Optional<User> findById(String id);

    /** Return all users in the system. */
    List<User> findAll();

    /** Delete a user by their unique identifier. */
    void deleteById(String id);

    /** Check whether a user with the given email already exists. */
    boolean existsByEmail(String email);
}
