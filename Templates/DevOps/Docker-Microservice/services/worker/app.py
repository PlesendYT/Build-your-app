"""
Background worker that processes jobs from the Redis queue.
Runs indefinitely, polling for new jobs every second.
"""

import asyncio
import json
import os
import time

import redis.asyncio as aioredis

# Redis connection URL from the environment
REDIS_URL = os.getenv("REDIS_URL", "redis://localhost:6379/0")


async def process_job(job: dict):
    """
    Simulate processing a job by sleeping briefly, then store the result.
    In a real application this would contain business logic.
    """
    job_id = job["id"]
    print(f"Processing job {job_id}...")

    # Simulate work with a short delay
    await asyncio.sleep(2)

    # Mark the job as completed and store the result
    job["status"] = "completed"
    job["result"] = f"Processed at {time.time()}"
    job["completed_at"] = time.time()

    r = await get_redis()
    await r.set(f"job:{job_id}", json.dumps(job))
    print(f"Job {job_id} completed.")


async def get_redis():
    """Return a shared Redis connection."""
    return aioredis.from_url(REDIS_URL, decode_responses=True)


async def main():
    """
    Main loop: block on BLPOP waiting for jobs, then process each one.
    Runs forever until the container is stopped.
    """
    r = await get_redis()
    print("Worker started. Waiting for jobs...")

    while True:
        # Block until a job appears on the "jobs" list (timeout=0 means forever)
        _, job_data = await r.blpop("jobs", timeout=0)
        job = json.loads(job_data)
        await process_job(job)


if __name__ == "__main__":
    asyncio.run(main())
