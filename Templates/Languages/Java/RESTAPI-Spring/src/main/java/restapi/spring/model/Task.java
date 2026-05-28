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
    public Task {
        if (title != null) title = title.trim();
    }

    public Task withId(Long id) {
        return new Task(id, title(), description(), completed());
    }

    public Task toggle() {
        return new Task(id(), title(), description(), !completed());
    }
}
