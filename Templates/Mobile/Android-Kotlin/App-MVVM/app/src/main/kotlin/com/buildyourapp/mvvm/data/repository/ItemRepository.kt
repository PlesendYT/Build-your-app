package com.buildyourapp.mvvm.data.repository

import com.buildyourapp.mvvm.data.dao.ItemDao
import com.buildyourapp.mvvm.data.model.Item
import kotlinx.coroutines.flow.Flow

class ItemRepository(private val itemDao: ItemDao) {

    val allItems: Flow<List<Item>> = itemDao.getAllItems()

    suspend fun addItem(title: String, description: String) {
        itemDao.insertItem(Item(title = title, description = description))
    }

    suspend fun removeItem(item: Item) {
        itemDao.deleteItem(item)
    }
}
