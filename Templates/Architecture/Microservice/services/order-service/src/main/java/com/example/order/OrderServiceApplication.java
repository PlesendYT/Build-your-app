/**
 * Spring Boot entry point for the Order microservice.
 * <p>
 * This microservice manages orders, order lines, and checkout flow.
 * It communicates with the User service for customer validation and
 * with the Inventory service for stock verification.
 */
package com.example.order;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
public class OrderServiceApplication {

    /**
     * Launch the Order microservice.
     *
     * @param args command-line arguments passed to the application
     */
    public static void main(String[] args) {
        SpringApplication.run(OrderServiceApplication.class, args);
    }
}
