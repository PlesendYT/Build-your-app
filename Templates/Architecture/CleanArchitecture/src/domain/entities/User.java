/**
 * Domain entity representing a user in the system.
 * <p>
 * Clean Architecture: Entities encapsulate enterprise-wide business rules.
 * This class is pure business logic with no framework dependencies.
 */
package com.example.domain.entities;

import java.util.Objects;
import java.util.UUID;

public class User {

    private final String id;
    private String name;
    private String email;

    public User(String name, String email) {
        this.id = UUID.randomUUID().toString();
        setName(name);
        setEmail(email);
    }

    public User(String id, String name, String email) {
        this.id = id;
        setName(name);
        setEmail(email);
    }

    /** Returns the unique identifier of this user. */
    public String getId() {
        return id;
    }

    /** Returns the user's display name. */
    public String getName() {
        return name;
    }

    /** Updates the user's display name. */
    public void setName(String name) {
        this.name = Objects.requireNonNull(name, "name must not be null");
    }

    /** Returns the user's email address. */
    public String getEmail() {
        return email;
    }

    /** Updates the user's email address. */
    public void setEmail(String email) {
        this.email = Objects.requireNonNull(email, "email must not be null");
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (!(o instanceof User)) return false;
        User user = (User) o;
        return id.equals(user.id);
    }

    @Override
    public int hashCode() {
        return Objects.hash(id);
    }
}
