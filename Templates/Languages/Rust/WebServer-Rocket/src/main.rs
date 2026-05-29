//! Rocket web server with health, info, and item-creation endpoints.

#[macro_use] extern crate rocket;

use rocket::{State, serde::json::Json};
use serde::{Deserialize, Serialize};
use std::sync::atomic::{AtomicU64, Ordering};

/// Shared state with an atomic request counter.
struct AppState {
    counter: AtomicU64,
}

/// Health-check response body.
#[derive(Serialize)]
struct Health {
    status: String,
    version: String,
}

/// Info endpoint response body.
#[derive(Serialize)]
struct Info {
    message: String,
    counter: u64,
}

/// Request body for creating a new item.
#[derive(Deserialize)]
struct CreateItem {
    name: String,
    value: String,
}

/// Response body after creating an item.
#[derive(Serialize)]
struct ItemResponse {
    id: u64,
    name: String,
    value: String,
}

/// Returns a health check with the current package version.
#[get("/health")]
fn health() -> Json<Health> {
    Json(Health {
        status: "ok".into(),
        version: env!("CARGO_PKG_VERSION").into(),
    })
}

/// Returns a greeting and an incrementing request counter.
#[get("/info")]
fn info(state: &State<AppState>) -> Json<Info> {
    let counter = state.counter.fetch_add(1, Ordering::SeqCst) + 1;
    Json(Info {
        message: "Rocket Server läuft / is running".into(),
        counter,
    })
}

/// Creates a new item from JSON input and returns it with a random ID.
#[post("/items", format = "json", data = "<body>")]
fn create_item(body: Json<CreateItem>) -> Json<ItemResponse> {
    let id = rand::random::<u64>();
    Json(ItemResponse {
        id,
        name: body.name.clone(),
        value: body.value.clone(),
    })
}

/// 404 catch-all handler.
#[catch(404)]
fn not_found() -> Json<serde_json::Value> {
    Json(serde_json::json!({"error": "Resource not found"}))
}

/// Entry point: builds and launches the Rocket application.
#[launch]
fn rocket() -> _ {
    rocket::build()
        .manage(AppState { counter: AtomicU64::new(0) })
        .mount("/", routes![health, info, create_item])
        .register("/", catchers![not_found])
}
