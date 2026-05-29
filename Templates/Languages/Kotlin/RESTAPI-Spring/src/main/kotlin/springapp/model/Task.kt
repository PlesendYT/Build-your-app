/**
 * Task entity mapped to the "tasks" database table.
 * Used by Spring Data JDBC with auto-generated ID and nullable description.
 */
package springapp.model

import org.springframework.data.annotation.Id
import org.springframework.data.relational.core.mapping.Table

@Table("tasks")
data class Task(
    @Id val id: Long? = null,
    val title: String,
    val description: String? = null,
    val completed: Boolean = false
)
