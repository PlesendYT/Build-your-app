/**
 * Android Todo app built with Jetpack Compose and ViewModel.
 * Supports adding items and toggling their completion status.
 */
package android.app

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.lifecycle.viewmodel.compose.viewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch

/** Data class representing a single todo item with an id, text, and completion flag. */
data class TodoItem(val id: Int, val text: String, val done: Boolean = false)

/** ViewModel that holds the todo list state and exposes add/toggle mutations via StateFlow. */
class TodoViewModel : androidx.lifecycle.ViewModel() {
    // Backing state flow initialized with two sample items
    private val _items = MutableStateFlow(
        listOf(TodoItem(1, "Compose lernen"), TodoItem(2, "Coroutines üben"))
    )
    /** Public read-only state flow of the todo list. */
    val items: StateFlow<List<TodoItem>> = _items.asStateFlow()
    private var nextId = 3

    /** Adds a new todo item with the given text. */
    fun add(text: String) {
        _items.value = _items.value + TodoItem(nextId++, text)
    }

    /** Toggles the completion status of the item with the given id. */
    fun toggle(id: Int) {
        _items.value = _items.value.map {
            if (it.id == id) it.copy(done = !it.done) else it
        }
    }
}

/** Main activity that renders the TodoApp composable inside a Material theme. */
class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            MaterialTheme {
                TodoApp()
            }
        }
    }
}

/** Composable that renders the full todo list UI including input field, FAB, and item list. */
@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun TodoApp(viewModel: TodoViewModel = viewModel()) {
    // Collect state from the ViewModel and track the text field input
    val items by viewModel.items.collectAsState()
    var text by remember { mutableStateOf("") }
    val scope = rememberCoroutineScope()

    Scaffold(
        topBar = { TopAppBar(title = { Text("Kotlin Android App") }) },
        floatingActionButton = {
            FloatingActionButton(onClick = {
                scope.launch { viewModel.add(text.ifBlank { "Neues Item" }) }
                text = ""
            }) { Text("+") }
        }
    ) { padding ->
        Column(modifier = Modifier.fillMaxSize().padding(padding).padding(16.dp)) {
            // Input field for new todo text
            OutlinedTextField(
                value = text,
                onValueChange = { text = it },
                label = { Text("Neues Todo") },
                modifier = Modifier.fillMaxWidth()
            )
            Spacer(Modifier.height(8.dp))
            // Scrollable list of todo items
            LazyColumn {
                items(items) { item ->
                    Card(
                        modifier = Modifier.fillMaxWidth().padding(vertical = 4.dp),
                        onClick = { viewModel.toggle(item.id) }
                    ) {
                        Row(
                            modifier = Modifier.padding(16.dp),
                            verticalAlignment = Alignment.CenterVertically
                        ) {
                            // Show checkmark or circle depending on completion state
                            Text(
                                text = if (item.done) "✓ " else "○ ",
                                style = MaterialTheme.typography.titleMedium
                            )
                            Text(item.text)
                        }
                    }
                }
            }
        }
    }
}
