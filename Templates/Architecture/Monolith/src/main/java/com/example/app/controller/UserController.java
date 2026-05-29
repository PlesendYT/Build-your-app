/**
 * REST controller for user management within the monolith.
 * <p>
 * All modules share the same process space, so the controller
 * can directly call service and repository classes without
 * inter-service communication overhead.
 */
package com.example.app.controller;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

@RestController
@RequestMapping("/api/users")
public class UserController {

    private final Map<String, Map<String, String>> users = new ConcurrentHashMap<>();

    /**
     * Register a new user.
     *
     * @param body the request payload with "name" and "email"
     * @return the created user record
     */
    @PostMapping
    public ResponseEntity<Map<String, String>> createUser(@RequestBody Map<String, String> body) {
        String id = String.valueOf(users.size() + 1);
        Map<String, String> user = Map.of("id", id, "name", body.get("name"), "email", body.get("email"));
        users.put(id, user);
        return ResponseEntity.status(HttpStatus.CREATED).body(user);
    }

    /**
     * Retrieve a user by ID.
     *
     * @param id the user's identifier
     * @return the user data or 404
     */
    @GetMapping("/{id}")
    public ResponseEntity<Map<String, String>> getUser(@PathVariable String id) {
        Map<String, String> user = users.get(id);
        if (user == null) return ResponseEntity.notFound().build();
        return ResponseEntity.ok(user);
    }

    /** List all registered users. */
    @GetMapping
    public ResponseEntity<?> listUsers() {
        return ResponseEntity.ok(users.values());
    }
}
