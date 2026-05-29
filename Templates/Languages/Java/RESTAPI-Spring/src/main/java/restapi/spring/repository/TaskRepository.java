/**
 * Spring Data JPA repository for Task entities.
 */
package restapi.spring.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import restapi.spring.model.Task;

/** Provides standard CRUD access to the tasks table. */
@Repository
public interface TaskRepository extends JpaRepository<Task, Long> {
}
