# ===== Stage 1: install dependencies =====
# Use official Node.js LTS image for a reproducible build environment
FROM node:20-alpine AS builder
WORKDIR /app

# Copy dependency manifests first to leverage Docker layer caching
COPY package.json ./
RUN npm install --production

# ===== Stage 2: runtime image =====
# Minimal alpine image that only contains the app and its production deps
FROM node:20-alpine AS runtime
WORKDIR /app

# Create a non-root user for security best practices
RUN addgroup -S appgroup && adduser -S appuser -G appgroup

# Copy only the production artifacts from the builder stage
COPY --from=builder /app/node_modules ./node_modules
COPY app/ ./app/

# Switch to non-root user before starting the application
USER appuser

# Expose the port the Express server listens on
EXPOSE 3000

# Default command: start the Node.js HTTP server
CMD ["node", "app/server.js"]
