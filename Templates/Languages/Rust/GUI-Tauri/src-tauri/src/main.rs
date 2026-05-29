//! Tauri backend commands for the GUI application.
//! Exposes `greet` and `add` functions to the frontend via the Tauri invoke system.

/// Returns a greeting string for the given name.
#[tauri::command]
fn greet(name: &str) -> String {
    format!("Hello, {name}! Willkommen bei Tauri!")
}

/// Adds two integers and returns the result.
#[tauri::command]
fn add(a: i32, b: i32) -> i32 {
    a + b
}

/// Entry point: registers the commands and starts the Tauri application.
fn main() {
    tauri::Builder::default()
        .invoke_handler(tauri::generate_handler![greet, add])
        .run(tauri::generate_context!())
        .expect("Error while running Tauri application");
}
