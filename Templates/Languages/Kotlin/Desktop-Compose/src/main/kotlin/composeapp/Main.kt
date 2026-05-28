package composeapp

import androidx.compose.desktop.ui.tooling.preview.Preview
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.compose.ui.window.Window
import androidx.compose.ui.window.application
import kotlinx.coroutines.launch
import java.io.File

data class Note(val title: String, val content: String)

@Composable
@Preview
fun NoteApp() {
    var notes by remember { mutableStateOf(listOf<Note>()) }
    var title by remember { mutableStateOf("") }
    var content by remember { mutableStateOf("") }
    val scope = rememberCoroutineScope()

    MaterialTheme {
        Row(modifier = Modifier.fillMaxSize().padding(16.dp)) {
            Column(modifier = Modifier.width(250.dp).fillMaxHeight()) {
                Text("Notizen", style = MaterialTheme.typography.headlineMedium)
                Spacer(Modifier.height(8.dp))
                LazyColumn(modifier = Modifier.weight(1f)) {
                    items(notes) { note ->
                        Card(modifier = Modifier.fillMaxWidth().padding(vertical = 4.dp)) {
                            Text(note.title, modifier = Modifier.padding(8.dp))
                        }
                    }
                }
            }
            Spacer(Modifier.width(16.dp))
            Column(modifier = Modifier.weight(1f)) {
                OutlinedTextField(value = title, onValueChange = { title = it },
                    label = { Text("Titel") }, modifier = Modifier.fillMaxWidth())
                Spacer(Modifier.height(8.dp))
                OutlinedTextField(value = content, onValueChange = { content = it },
                    label = { Text("Inhalt") }, modifier = Modifier.fillMaxWidth().height(200.dp))
                Spacer(Modifier.height(8.dp))
                Button(onClick = {
                    if (title.isNotBlank()) {
                        notes = notes + Note(title, content)
                        title = ""; content = ""
                    }
                }) { Text("Hinzufügen") }
            }
        }
    }
}

fun main() = application {
    Window(onCloseRequest = ::exitApplication, title = "Compose Desktop") {
        NoteApp()
    }
}
