package mp.android

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.layout.*
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import shared.Greeting

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            MaterialTheme {
                val greeting = remember { Greeting() }
                val data = remember { greeting.greet("KMP") }

                Column(modifier = Modifier.fillMaxSize().padding(16.dp)) {
                    Text(data.message, style = MaterialTheme.typography.headlineMedium)
                    Text("Plattform: ${data.platform}")
                }
            }
        }
    }
}
