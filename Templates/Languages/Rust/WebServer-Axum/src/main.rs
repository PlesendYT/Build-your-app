//! Axum web server with health, info, and item-creation endpoints.
//! Includes CORS support and structured logging via `tracing`.

use axum::{
    extract::State,
    http::StatusCode,
    response::IntoResponse,
    routing::{get, post},
    Json, Router,
};
use serde::{Deserialize, Serialize};
use std::sync::atomic::{AtomicU64, Ordering};
use std::sync::Arc;
use tower_http::cors::CorsLayer;
use tracing_subscriber;

/// Shared application state with an atomic request counter.
#[derive(Clone)]
struct AppState {
    counter: Arc<AtomicU64>,
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
    id: String,
    name: String,
    value: String,
}

/// Returns a health check with the current package version.
async fn health() -> Json<Health> {
    Json(Health {
        status: "ok".into(),
        version: env!("CARGO_PKG_VERSION").into(),
    })
}

/// Returns a greeting and an incrementing request counter.
async fn info(State(state): State<AppState>) -> Json<Info> {
    let counter = state.counter.fetch_add(1, Ordering::SeqCst) + 1;
    Json(Info {
        message: "Axum Server läuft / is running".into(),
        counter,
    })
}

/// Creates a new item from JSON input and returns it with a generated UUID.
async fn create_item(Json(body): Json<CreateItem>) -> impl IntoResponse {
    let id = uuid::Uuid::new_v4();
    (
        StatusCode::CREATED,
        Json(ItemResponse {
            id: id.to_string(),
            name: body.name,
            value: body.value,
        }),
    )
}

/// 404 catch-all handler.
async fn fallback() -> impl IntoResponse {
    (
        StatusCode::NOT_FOUND,
        Json(serde_json::json!({"error": "Resource not found"})),
    )
}

/// Entry point: initialises tracing, builds the router, and starts the server.
#[tokio::main]
async fn main() -> anyhow::Result<()> {
    tracing_subscriber::fmt::init();

    let state = AppState {
        counter: Arc::new(AtomicU64::new(0)),
    };

    let app = Router::new()
        .route("/health", get(health))
        .route("/info", get(info))
        .route("/items", post(create_item))
        .fallback(fallback)
        .layer(CorsLayer::permissive())
        .with_state(state);

    let listener = tokio::net::TcpListener::bind("0.0.0.0:3000").await?;
    tracing::info!("Server listening on 0.0.0.0:3000");
    axum::serve(listener, app).await?;
    Ok(())
}
