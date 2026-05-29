/**
 * Spring Boot entry point for a monolithic application.
 * <p>
 * In a monolithic architecture all modules (users, orders, products)
 * are deployed as a single unit. This simplifies development and
 * deployment at the cost of independent scalability.
 */
package com.example.app;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
public class Application {

    /**
     * Launch the monolithic application.
     *
     * @param args command-line arguments passed to the application
     */
    public static void main(String[] args) {
        SpringApplication.run(Application.class, args);
    }
}
