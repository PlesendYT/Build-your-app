/**
 * Spring Boot entry point for the User microservice.
 * <p>
 * This microservice handles user registration, authentication, and
 * profile management. It communicates with other services via REST
 * and message brokers.
 */
package com.example.user;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
public class UserServiceApplication {

    /**
     * Launch the User microservice.
     *
     * @param args command-line arguments passed to the application
     */
    public static void main(String[] args) {
        SpringApplication.run(UserServiceApplication.class, args);
    }
}
