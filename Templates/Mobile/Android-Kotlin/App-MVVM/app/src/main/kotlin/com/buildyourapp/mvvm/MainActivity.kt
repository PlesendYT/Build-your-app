package com.buildyourapp.mvvm

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.lifecycle.viewmodel.compose.viewModel
import com.buildyourapp.mvvm.ui.viewmodel.ItemViewModel

/** Main entry point for the MVVM template. Sets up Material theme and renders the main screen. */
class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            MaterialTheme {
                Surface(modifier = Modifier.fillMaxSize()) {
                    MainScreen()
                }
            }
        }
    }
}

/** Main screen with a form to add items (title + description) and a list displaying all stored items. */
@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun MainScreen(viewModel: ItemViewModel = viewModel()) {
    // Collect items as state from the ViewModel and hold local form state
    val items by viewModel.items.collectAsState()
    var title by remember { mutableStateOf("") }
    var description by remember { mutableStateOf("") }

    Scaffold(
        topBar = { TopAppBar(title = { Text("MVVM App") }) }
    ) { padding ->
        Column(modifier = Modifier.padding(padding).padding(16.dp)) {
            // Input fields for item title and description
            OutlinedTextField(value = title, onValueChange = { title = it }, label = { Text("Title") })
            Spacer(modifier = Modifier.height(8.dp))
            OutlinedTextField(value = description, onValueChange = { description = it }, label = { Text("Description") })
            Spacer(modifier = Modifier.height(8.dp))
            // Add button clears inputs after inserting the item via ViewModel
            Button(onClick = { viewModel.addItem(title, description); title = ""; description = "" }) {
                Text("Add Item")
            }
            Spacer(modifier = Modifier.height(16.dp))
            // Scrollable list of items displayed in cards
            LazyColumn {
                items(items) { item ->
                    Card(modifier = Modifier.fillMaxWidth().padding(vertical = 4.dp)) {
                        Column(modifier = Modifier.padding(12.dp)) {
                            Text(text = item.title, style = MaterialTheme.typography.titleMedium)
                            Text(text = item.description, style = MaterialTheme.typography.bodyMedium)
                        }
                    }
                }
            }
        }
    }
}
