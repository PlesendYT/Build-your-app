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

    @GetMapping
    suspend fun getAll(): Flow<Task> = repository.findAll()

    @GetMapping("/{id}")
    suspend fun getById(@PathVariable id: Long): ResponseEntity<Task> {
        val task = repository.findById(id) ?: return ResponseEntity.notFound().build()
        return ResponseEntity.ok(task)
    }

    @PostMapping
    suspend fun create(@RequestBody task: Task): ResponseEntity<Task> {
        val saved = repository.save(task.copy(id = null))
        return ResponseEntity.status(HttpStatus.CREATED).body(saved)
    }

    @PutMapping("/{id}")
    suspend fun update(@PathVariable id: Long, @RequestBody task: Task): ResponseEntity<Task> {
        if (!repository.existsById(id)) return ResponseEntity.notFound().build()
        val saved = repository.save(task.copy(id = id))
        return ResponseEntity.ok(saved)
    }

    @DeleteMapping("/{id}")
    suspend fun delete(@PathVariable id: Long): ResponseEntity<Unit> {
        if (!repository.existsById(id)) return ResponseEntity.notFound().build()
        repository.deleteById(id)
        return ResponseEntity.noContent().build()
    }
}
