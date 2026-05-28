#[tauri::command]
fn greet(name: &str) -> String {
    format!("Hello, {name}! Willkommen bei Tauri!")
}

#[tauri::command]
fn add(a: i32, b: i32) -> i32 {
    a + b
}

fn main() {
    tauri::Builder::default()
        .invoke_handler(tauri::generate_handler![greet, add])
        .run(tauri::generate_context!())
        .expect("Error while running Tauri application");
}
