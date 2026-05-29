/**
 * REST controller exposing CRUD endpoints for the Task resource.
 * Uses coroutine-based repository methods for non-blocking database access.
 */
package springapp.controller

import kotlinx.coroutines.flow.*
import org.springframework.http.HttpStatus
import org.springframework.http.ResponseEntity
import org.springframework.web.bind.annotation.*
import springapp.model.Task
import springapp.repository.TaskRepository

@RestController
@RequestMapping("/api/tasks")
class TaskController(private val repository: TaskRepository) {

    /** Returns a flow of all tasks. */
    @GetMapping
    suspend fun getAll(): Flow<Task> = repository.findAll()

    /** Returns a single task by ID, or 404 if not found. */
    @GetMapping("/{id}")
    suspend fun getById(@PathVariable id: Long): ResponseEntity<Task> {
        val task = repository.findById(id) ?: return ResponseEntity.notFound().build()
        return ResponseEntity.ok(task)
    }

    /** Creates a new task and returns it with HTTP 201. */
    @PostMapping
    suspend fun create(@RequestBody task: Task): ResponseEntity<Task> {
        val saved = repository.save(task.copy(id = null))
        return ResponseEntity.status(HttpStatus.CREATED).body(saved)
    }

    /** Updates an existing task by ID, or returns 404 if not found. */
    @PutMapping("/{id}")
    suspend fun update(@PathVariable id: Long, @RequestBody task: Task): ResponseEntity<Task> {
        if (!repository.existsById(id)) return ResponseEntity.notFound().build()
        val saved = repository.save(task.copy(id = id))
        return ResponseEntity.ok(saved)
    }

    /** Deletes a task by ID, returns 204 on success or 404 if not found. */
    @DeleteMapping("/{id}")
    suspend fun delete(@PathVariable id: Long): ResponseEntity<Unit> {
        if (!repository.existsById(id)) return ResponseEntity.notFound().build()
        repository.deleteById(id)
        return ResponseEntity.noContent().build()
    }
}
