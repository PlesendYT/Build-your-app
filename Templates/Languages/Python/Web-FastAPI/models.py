"""Pydantic models for the FastAPI application."""

from __future__ import annotations

from pydantic import BaseModel, Field


class GreetingRequest(BaseModel):
    """Request model for greeting endpoint."""
    name: str = Field(default="World", min_length=1, max_length=100)


class GreetingResponse(BaseModel):
    """Response model for greeting endpoint."""
    message: str
