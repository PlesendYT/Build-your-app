/**
 * Spring REST controller for task CRUD operations.
 */
package restapi.spring.controller;

import jakarta.validation.Valid;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import restapi.spring.model.Task;
import restapi.spring.repository.TaskRepository;

import java.util.List;

/** Exposes /api/tasks endpoints with full CRUD and toggle-completed support. */
@RestController
@RequestMapping("/api/tasks")
public record TaskController(TaskRepository repository) {

    /** Returns all tasks. */
    @GetMapping
    public List<Task> getAll() {
        return repository.findAll();
    }

    /** Returns a single task by ID, or 404. */
    @GetMapping("/{id}")
    public ResponseEntity<Task> getById(@PathVariable Long id) {
        return repository.findById(id)
            .map(ResponseEntity::ok)
            .orElse(ResponseEntity.notFound().build());
    }

    /** Creates a new task and returns it with HTTP 201. */
    @PostMapping
    public ResponseEntity<Task> create(@Valid @RequestBody Task task) {
        var saved = repository.save(task.withId(null));
        return ResponseEntity.status(HttpStatus.CREATED).body(saved);
    }

    /** Replaces an existing task; returns 404 if not found. */
    @PutMapping("/{id}")
    public ResponseEntity<Task> update(@PathVariable Long id, @Valid @RequestBody Task task) {
        if (!repository.existsById(id)) {
            return ResponseEntity.notFound().build();
        }
        var saved = repository.save(task.withId(id));
        return ResponseEntity.ok(saved);
    }

    /** Toggles the completed flag of a task. */
    @PatchMapping("/{id}/toggle")
    public ResponseEntity<Task> toggle(@PathVariable Long id) {
        return repository.findById(id)
            .map(task -> {
                var toggled = repository.save(task.toggle());
                return ResponseEntity.ok(toggled);
            })
            .orElse(ResponseEntity.notFound().build());
    }

    /** Deletes a task by ID; returns 404 if not found. */
    @DeleteMapping("/{id}")
    public ResponseEntity<Void> delete(@PathVariable Long id) {
        if (!repository.existsById(id)) {
            return ResponseEntity.notFound().build();
        }
        repository.deleteById(id);
        return ResponseEntity.noContent().build();
    }
}
