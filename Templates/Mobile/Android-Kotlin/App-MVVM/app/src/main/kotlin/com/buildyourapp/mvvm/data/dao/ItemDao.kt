package com.buildyourapp.mvvm.data.dao

import androidx.room.Dao
import androidx.room.Delete
import androidx.room.Insert
import androidx.room.Query
import com.buildyourapp.mvvm.data.model.Item
import kotlinx.coroutines.flow.Flow

/** Room DAO for accessing the items table. Provides query, insert, and delete operations. */
@Dao
interface ItemDao {
    /** Returns a reactive Flow of all items ordered by creation time (newest first). */
    @Query("SELECT * FROM items ORDER BY createdAt DESC")
    fun getAllItems(): Flow<List<Item>>

    /** Inserts a new item into the database. */
    @Insert
    suspend fun insertItem(item: Item)

    /** Deletes an existing item from the database. */
    @Delete
    suspend fun deleteItem(item: Item)
}
