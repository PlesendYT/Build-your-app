# Python microservice Dockerfile
# Uses a slim base with a virtual environment for dependency isolation
FROM python:3.12-slim AS builder

WORKDIR /app

# Install system dependencies required for building Python packages
RUN apt-get update && apt-get install -y --no-install-recommends \
    gcc && \
    rm -rf /var/lib/apt/lists/*

# Copy and install Python dependencies
COPY requirements.txt .
RUN pip install --no-cache-dir --user -r requirements.txt

# --- Runtime stage ---
FROM python:3.12-slim AS runtime

WORKDIR /app

# Copy installed packages from the builder stage
COPY --from=builder /root/.local /root/.local

# Ensure the local bin directory is on PATH
ENV PATH=/root/.local/bin:$PATH

# Copy the service code into the container
COPY services/ ./services/

# Expose the FastAPI default port
EXPOSE 8000

# Start the FastAPI server using Uvicorn
CMD ["uvicorn", "services.api.app:app", "--host", "0.0.0.0", "--port", "8000"]
