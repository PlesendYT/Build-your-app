use axum::{extract::State, routing::{get, post}, Json, Router};
use serde::{Deserialize, Serialize};
use sqlx::sqlite::{SqlitePool, SqlitePoolOptions};
use std::sync::Arc;

#[derive(Clone)]
struct AppState { pool: SqlitePool }

#[derive(Serialize)]
struct Item { id: i64, name: String }

#[derive(Deserialize)]
struct NewItem { name: String }

async fn list(State(state): State<Arc<AppState>>) -> Json<Vec<Item>> {
    let rows = sqlx::query!("SELECT id, name FROM items ORDER BY id")
        .fetch_all(&state.pool).await.unwrap_or_default();
    Json(rows.into_iter().map(|r| Item { id: r.id, name: r.name }).collect())
}

async fn create(State(state): State<Arc<AppState>>, Json(body): Json<NewItem>) -> Json<Item> {
    let row = sqlx::query!("INSERT INTO items(name) VALUES (?1) RETURNING id, name", body.name)
        .fetch_one(&state.pool).await.unwrap();
    Json(Item { id: row.id, name: row.name })
}

#[tokio::main]
async fn main() -> anyhow::Result<()> {
    let pool = SqlitePoolOptions::new().connect("sqlite::memory:").await?;
    sqlx::query("CREATE TABLE items (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL)")
        .execute(&pool).await?;
    let state = Arc::new(AppState { pool });
    let app = Router::new()
        .route("/items", get(list).post(create))
        .with_state(state);
    let listener = tokio::net::TcpListener::bind("0.0.0.0:8080").await?;
    axum::serve(listener, app).await?;
    Ok(())
}