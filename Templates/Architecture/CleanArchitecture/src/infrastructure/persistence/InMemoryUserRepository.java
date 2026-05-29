/**
 * In-memory implementation of UserRepository for development and testing.
 * <p>
 * Infrastructure layer: contains concrete implementations of interfaces
 * defined in the domain layer. Easily replaceable with a database-backed
 * implementation without affecting the rest of the system.
 */
package com.example.infrastructure.persistence;

import com.example.domain.entities.User;
import com.example.domain.repositories.UserRepository;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;

public class InMemoryUserRepository implements UserRepository {

    private final Map<String, User> store = new HashMap<>();

    @Override
    public User save(User user) {
        store.put(user.getId(), user);
        return user;
    }

    @Override
    public Optional<User> findById(String id) {
        return Optional.ofNullable(store.get(id));
    }

    @Override
    public List<User> findAll() {
        return new ArrayList<>(store.values());
    }

    @Override
    public void deleteById(String id) {
        store.remove(id);
    }

    @Override
    public boolean existsByEmail(String email) {
        return store.values().stream().anyMatch(u -> u.getEmail().equals(email));
    }
}
