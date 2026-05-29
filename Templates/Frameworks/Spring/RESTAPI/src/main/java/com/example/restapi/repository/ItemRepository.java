/**
 * Spring Data JPA repository for Item entities.
 */
package com.example.restapi.repository;

import com.example.restapi.model.Item;
import org.springframework.data.jpa.repository.JpaRepository;

/** Provides standard CRUD access to the items table. */
public interface ItemRepository extends JpaRepository<Item, Long> {
}
