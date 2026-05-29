/**
 * JPA entity record representing a task with title, description, and
 * completion status.
 */
package restapi.spring.model;

import jakarta.persistence.*;
import jakarta.validation.constraints.NotBlank;

@Entity
@Table(name = "tasks")
public record Task(
    @Id @GeneratedValue(strategy = GenerationType.IDENTITY) Long id,
    @NotBlank String title,
    String description,
    boolean completed
) {
    /** Trims the title on construction (null-safe). */
    public Task {
        if (title != null) title = title.trim();
    }

    /** Returns a copy of this task with a new ID (null for insert). */
    public Task withId(Long id) {
        return new Task(id, title(), description(), completed());
    }

    /** Returns a copy with the completed flag flipped. */
    public Task toggle() {
        return new Task(id(), title(), description(), !completed());
    }
}
