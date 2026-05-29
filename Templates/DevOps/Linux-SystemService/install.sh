#!/bin/bash
# install.sh — Build the Go binary and install it as a systemd service
# Run as root: sudo ./install.sh

set -euo pipefail

# Determine the script directory for reliable path resolution
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
BINARY_NAME="myapp"
SERVICE_NAME="myapp.service"

echo "=== Installing $SERVICE_NAME ==="

# Step 1: Build the Go binary with version information
echo ">>> Building Go binary..."
cd "$SCRIPT_DIR"

# Embed build timestamp and version into the binary via ldflags
BUILD_TIME="$(date -u '+%Y-%m-%dT%H:%M:%SZ')"
go build -ldflags "-X main.BuildVersion=1.0.0 -X main.BuildTime=$BUILD_TIME" \
  -o "$BINARY_NAME" ./app/main.go

# Step 2: Install the binary to /usr/local/bin
echo ">>> Installing binary to /usr/local/bin/$BINARY_NAME..."
sudo install -m 0755 "$BINARY_NAME" /usr/local/bin/

# Step 3: Install the systemd unit file
echo ">>> Installing systemd unit file..."
sudo install -m 0644 "$SERVICE_NAME" /etc/systemd/system/

# Step 4: Reload systemd, enable and start the service
echo ">>> Reloading systemd daemon..."
sudo systemctl daemon-reload

echo ">>> Enabling service to start on boot..."
sudo systemctl enable "$SERVICE_NAME"

echo ">>> Starting service..."
sudo systemctl start "$SERVICE_NAME"

# Step 5: Verify the service is running
echo ">>> Checking service status..."
sleep 1
sudo systemctl status "$SERVICE_NAME" --no-pager

echo "=== Installation complete ==="
echo "Service: $SERVICE_NAME"
echo "Binary:  /usr/local/bin/$BINARY_NAME"
echo "Logs:    journalctl -u $SERVICE_NAME -f"
