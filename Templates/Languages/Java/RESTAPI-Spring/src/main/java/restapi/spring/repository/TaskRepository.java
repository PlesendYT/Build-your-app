package restapi.spring.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import restapi.spring.model.Task;

@Repository
public interface TaskRepository extends JpaRepository<Task, Long> {
}
