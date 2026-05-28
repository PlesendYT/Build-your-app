"""FastAPI application entry point."""

from __future__ import annotations

from fastapi import FastAPI
from models import GreetingRequest, GreetingResponse

app = FastAPI(title="FastAPI App", version="1.0.0")


@app.get("/")
def read_root() -> dict[str, str]:
    """Root endpoint."""
    return {"message": "Hello, World!"}


@app.get("/greet/{name}")
def greet_path(name: str) -> GreetingResponse:
    """Greet a user by path parameter."""
    return GreetingResponse(message=f"Hello, {name}!")


@app.post("/greet")
def greet_body(body: GreetingRequest) -> GreetingResponse:
    """Greet a user by POST body."""
    name = body.name.strip() or "World"
    return GreetingResponse(message=f"Hello, {name}!")


def main() -> None:
    import uvicorn
    uvicorn.run("main:app", host="127.0.0.1", port=8000, reload=True)


if __name__ == "__main__":
    main()
