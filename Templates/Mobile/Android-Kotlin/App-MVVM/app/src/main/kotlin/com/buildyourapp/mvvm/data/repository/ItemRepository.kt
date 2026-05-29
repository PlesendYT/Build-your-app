package com.buildyourapp.mvvm.data.repository

import com.buildyourapp.mvvm.data.dao.ItemDao
import com.buildyourapp.mvvm.data.model.Item
import kotlinx.coroutines.flow.Flow

/** Repository that abstracts the data source (ItemDao) and provides a clean API for item operations. */
class ItemRepository(private val itemDao: ItemDao) {

    /** Reactive stream of all items from the database. */
    val allItems: Flow<List<Item>> = itemDao.getAllItems()

    /** Creates and inserts a new item with the given [title] and [description]. */
    suspend fun addItem(title: String, description: String) {
        itemDao.insertItem(Item(title = title, description = description))
    }

    /** Removes the given [item] from the database. */
    suspend fun removeItem(item: Item) {
        itemDao.deleteItem(item)
    }
}
