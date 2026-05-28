use actix_web::{web, App, HttpServer, HttpResponse, middleware};
use serde::{Deserialize, Serialize};
use std::sync::Mutex;

struct AppState {
    counter: Mutex<u64>,
}

#[derive(Serialize)]
struct HealthResponse {
    status: String,
    version: String,
}

#[derive(Serialize)]
struct InfoResponse {
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

async fn health() -> HttpResponse {
    HttpResponse::Ok().json(HealthResponse {
        status: "ok".into(),
        version: env!("CARGO_PKG_VERSION").into(),
    })
}

async fn info(data: web::Data<AppState>) -> HttpResponse {
    let mut counter = data.counter.lock().unwrap();
    *counter += 1;
    HttpResponse::Ok().json(InfoResponse {
        message: "Actix-Web Server läuft / is running".into(),
        counter: *counter,
    })
}

async fn create_item(body: web::Json<CreateItem>) -> HttpResponse {
    let id = rand::random::<u64>();
    HttpResponse::Created().json(ItemResponse {
        id,
        name: body.name.clone(),
        value: body.value.clone(),
    })
}

async fn not_found() -> HttpResponse {
    HttpResponse::NotFound().json(serde_json::json!({
        "error": "Resource not found"
    }))
}

#[actix_web::main]
async fn main() -> std::io::Result<()> {
    let state = web::Data::new(AppState {
        counter: Mutex::new(0),
    });

    println!("Server starting on http://0.0.0.0:8080");

    HttpServer::new(move || {
        App::new()
            .app_data(state.clone())
            .route("/health", web::get().to(health))
            .route("/info", web::get().to(info))
            .route("/items", web::post().to(create_item))
            .default_service(web::route().to(not_found))
    })
    .bind("0.0.0.0:8080")?
    .run()
    .await
}
