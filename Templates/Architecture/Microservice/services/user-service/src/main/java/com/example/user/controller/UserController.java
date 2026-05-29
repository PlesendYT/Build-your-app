/**
 * REST controller for user-related endpoints.
 * <p>
 * Exposes CRUD operations for user management. Each microservice
 * owns its data and exposes a bounded-context API.
 */
package com.example.user.controller;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

@RestController
@RequestMapping("/api/users")
public class UserController {

    // In-memory user store for demonstration purposes
    private final Map<String, Map<String, String>> users = new ConcurrentHashMap<>();

    /**
     * Create a new user.
     *
     * @param body a map containing "name" and "email" fields
     * @return the created user with a generated ID
     */
    @PostMapping
    public ResponseEntity<Map<String, String>> createUser(@RequestBody Map<String, String> body) {
        String id = String.valueOf(users.size() + 1);
        Map<String, String> user = Map.of(
            "id", id, "name", body.get("name"), "email", body.get("email")
        );
        users.put(id, user);
        return ResponseEntity.status(HttpStatus.CREATED).body(user);
    }

    /**
     * Retrieve a user by their unique identifier.
     *
     * @param id the user's UUID
     * @return the user data or 404 if not found
     */
    @GetMapping("/{id}")
    public ResponseEntity<Map<String, String>> getUser(@PathVariable String id) {
        Map<String, String> user = users.get(id);
        if (user == null) {
            return ResponseEntity.notFound().build();
        }
        return ResponseEntity.ok(user);
    }

    /**
     * List all registered users.
     *
     * @return a collection of all user records
     */
    @GetMapping
    public ResponseEntity<?> listUsers() {
        return ResponseEntity.ok(users.values());
    }
}
