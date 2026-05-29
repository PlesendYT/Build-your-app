/**
 * Spring Boot application entry point.
 * Bootstraps the embedded web server and auto-configures Spring context.
 */
package springapp

import org.springframework.boot.autoconfigure.SpringBootApplication
import org.springframework.boot.runApplication

@SpringBootApplication
class Application

fun main(args: Array<String>) {
    runApplication<Application>(*args)
}
