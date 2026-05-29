package com.buildyourapp.compose

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.animation.animateColorAsState
import androidx.compose.foundation.layout.*
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Home
import androidx.compose.material.icons.filled.Settings
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController

/** Main entry point for the Compose Advanced template. Wraps the app in a dark/light theme. */
class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        // Detect system dark theme and render the composable app inside it
        setContent {
            AppTheme(darkTheme = isSystemInDarkTheme()) {
                Surface(modifier = Modifier.fillMaxSize()) {
                    ComposeApp()
                }
            }
        }
    }
}

/** Applies a Material3 color scheme switching between dark and light based on [darkTheme]. */
@Composable
fun AppTheme(darkTheme: Boolean, content: @Composable () -> Unit) {
    val colorScheme = if (darkTheme) darkColorScheme() else lightColorScheme()
    MaterialTheme(colorScheme = colorScheme, content = content)
}

/** Top-level composable with a bottom navigation bar and NavHost routing to Home/Settings. */
@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun ComposeApp() {
    val navController = rememberNavController()
    // Scaffold provides the top app bar, bottom navigation bar, and content area
    Scaffold(
        topBar = { TopAppBar(title = { Text("Compose Advanced") }) },
        bottomBar = {
            NavigationBar {
                NavigationBarItem(
                    icon = { Icon(Icons.Default.Home, contentDescription = "Home") },
                    label = { Text("Home") },
                    selected = true,
                    onClick = { navController.navigate("home") }
                )
                NavigationBarItem(
                    icon = { Icon(Icons.Default.Settings, contentDescription = "Settings") },
                    label = { Text("Settings") },
                    selected = false,
                    onClick = { navController.navigate("settings") }
                )
            }
        }
    ) { padding ->
        // Define navigation graph with home and settings destinations
        NavHost(
            navController = navController,
            startDestination = "home",
            modifier = Modifier.padding(padding)
        ) {
            composable("home") { HomeScreen() }
            composable("settings") { SettingsScreen() }
        }
    }
}

/** Home screen with a counter button that animates its background color on each tap. */
@Composable
fun HomeScreen() {
    var count by remember { mutableIntStateOf(0) }
    // Animate between primary and tertiary color based on even/odd count
    val color by animateColorAsState(
        targetValue = if (count % 2 == 0) MaterialTheme.colorScheme.primary
        else MaterialTheme.colorScheme.tertiary, label = "color"
    )

    Column(
        modifier = Modifier.fillMaxSize().padding(24.dp),
        verticalArrangement = Arrangement.Center
    ) {
        Button(
            onClick = { count++ },
            modifier = Modifier.fillMaxWidth(),
            colors = ButtonDefaults.buttonColors(containerColor = color)
        ) {
            Text("Count: $count")
        }
    }
}

/** Settings screen that displays a simple headline. */
@Composable
fun SettingsScreen() {
    Text("Settings Page", style = MaterialTheme.typography.headlineMedium)
}
