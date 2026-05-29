"""
FastAPI microservice that accepts jobs and pushes them to a Redis queue.
The worker service pops jobs from the queue and processes them asynchronously.
"""

import json
import os
import uuid

import redis.asyncio as aioredis
from fastapi import FastAPI, HTTPException

# Create the FastAPI application instance
app = FastAPI(title="Microservice API", version="1.0.0")

# Redis connection URL from environment, with a fallback default
REDIS_URL = os.getenv("REDIS_URL", "redis://localhost:6379/0")

# Lazy-initialized Redis client
redis_client = None


async def get_redis():
    """Return a singleton Redis connection, creating it if necessary."""
    global redis_client
    if redis_client is None:
        redis_client = aioredis.from_url(REDIS_URL, decode_responses=True)
    return redis_client


@app.on_event("startup")
async def startup():
    """Verify Redis connectivity when the service starts."""
    r = await get_redis()
    await r.ping()


@app.get("/health")
async def health():
    """Health check endpoint for orchestrators and load balancers."""
    return {"status": "healthy"}


@app.post("/jobs")
async def create_job(payload: dict):
    """
    Accept a job payload, assign a unique ID, and enqueue it in Redis.
    Returns the job ID so the caller can track progress.
    """
    job_id = str(uuid.uuid4())
    job = {"id": job_id, "data": payload, "status": "queued"}

    r = await get_redis()
    # Push the job onto the "jobs" list in Redis
    await r.lpush("jobs", json.dumps(job))

    return {"job_id": job_id, "status": "queued"}


@app.get("/jobs/{job_id}")
async def get_job(job_id: str):
    """
    Retrieve the current status and result of a job from Redis.
    Raises 404 if the job key does not exist.
    """
    r = await get_redis()
    data = await r.get(f"job:{job_id}")
    if data is None:
        raise HTTPException(status_code=404, detail="Job not found")
    return json.loads(data)
