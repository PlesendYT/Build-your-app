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

class ItemViewModel(application: Application) : AndroidViewModel(application) {
    private val repository: ItemRepository

    val items: StateFlow<List<com.buildyourapp.mvvm.data.model.Item>>

    init {
        val dao = AppDatabase.getInstance(application).itemDao()
        repository = ItemRepository(dao)
        items = repository.allItems.stateIn(
            viewModelScope,
            SharingStarted.WhileSubscribed(5000),
            emptyList()
        )
    }

    fun addItem(title: String, description: String) {
        viewModelScope.launch {
            repository.addItem(title, description)
        }
    }

    fun deleteItem(item: com.buildyourapp.mvvm.data.model.Item) {
        viewModelScope.launch {
            repository.removeItem(item)
        }
    }
}
