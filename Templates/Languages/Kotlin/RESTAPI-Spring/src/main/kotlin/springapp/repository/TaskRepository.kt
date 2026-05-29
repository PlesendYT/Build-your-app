/**
 * Spring Data repository for Task entities.
 * Extends CoroutineCrudRepository to provide non-blocking CRUD operations.
 */
package springapp.repository

import org.springframework.data.repository.kotlin.CoroutineCrudRepository
import org.springframework.stereotype.Repository
import springapp.model.Task

@Repository
interface TaskRepository : CoroutineCrudRepository<Task, Long>
