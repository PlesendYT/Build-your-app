package com.buildyourapp.mvvm.ui.viewmodel

import android.app.Application
import androidx.lifecycle.AndroidViewModel
import androidx.lifecycle.viewModelScope
import com.buildyourapp.mvvm.data.db.AppDatabase
import com.buildyourapp.mvvm.data.repository.ItemRepository
import kotlinx.coroutines.flow.SharingStarted
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.stateIn
import kotlinx.coroutines.launch

/** ViewModel that bridges the UI layer with the repository. Exposes items as a StateFlow and provides add/delete actions. */
class ItemViewModel(application: Application) : AndroidViewModel(application) {
    private val repository: ItemRepository

    /** StateFlow of all items, shared with a 5-second subscriber timeout. */
    val items: StateFlow<List<com.buildyourapp.mvvm.data.model.Item>>

    init {
        // Initialize the database, DAO, and repository, then convert the Flow to a StateFlow
        val dao = AppDatabase.getInstance(application).itemDao()
        repository = ItemRepository(dao)
        items = repository.allItems.stateIn(
            viewModelScope,
            SharingStarted.WhileSubscribed(5000),
            emptyList()
        )
    }

    /** Inserts a new item with the given [title] and [description] via the repository. */
    fun addItem(title: String, description: String) {
        viewModelScope.launch {
            repository.addItem(title, description)
        }
    }

    /** Deletes the given [item] via the repository. */
    fun deleteItem(item: com.buildyourapp.mvvm.data.model.Item) {
        viewModelScope.launch {
            repository.removeItem(item)
        }
    }
}
