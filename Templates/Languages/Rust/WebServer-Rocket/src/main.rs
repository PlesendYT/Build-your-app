#[macro_use] extern crate rocket;

use rocket::{State, serde::json::Json};
use serde::{Deserialize, Serialize};
use std::sync::atomic::{AtomicU64, Ordering};

struct AppState {
    counter: AtomicU64,
}

#[derive(Serialize)]
struct Health {
    status: String,
    version: String,
}

#[derive(Serialize)]
struct Info {
    message: String,
    counter: u64,
}

#[derive(Deserialize)]
struct CreateItem {
    name: String,
    value: String,
}

#[derive(Serialize)]
struct ItemResponse {
    id: u64,
    name: String,
    value: String,
}

#[get("/health")]
fn health() -> Json<Health> {
    Json(Health {
        status: "ok".into(),
        version: env!("CARGO_PKG_VERSION").into(),
    })
}

#[get("/info")]
fn info(state: &State<AppState>) -> Json<Info> {
    let counter = state.counter.fetch_add(1, Ordering::SeqCst) + 1;
    Json(Info {
        message: "Rocket Server läuft / is running".into(),
        counter,
    })
}

#[post("/items", format = "json", data = "<body>")]
fn create_item(body: Json<CreateItem>) -> Json<ItemResponse> {
    let id = rand::random::<u64>();
    Json(ItemResponse {
        id,
        name: body.name.clone(),
        value: body.value.clone(),
    })
}

#[catch(404)]
fn not_found() -> Json<serde_json::Value> {
    Json(serde_json::json!({"error": "Resource not found"}))
}

#[launch]
fn rocket() -> _ {
    rocket::build()
        .manage(AppState { counter: AtomicU64::new(0) })
        .mount("/", routes![health, info, create_item])
        .register("/", catchers![not_found])
}
