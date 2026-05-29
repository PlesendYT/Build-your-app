# Makefile for the Linux-SystemService template
# Provides targets for building, installing, and managing the systemd service

# Binary name and output path
BINARY := myapp
INSTALL_PATH := /usr/local/bin/$(BINARY)
SERVICE_FILE := myapp.service

# Build-time variables injected via ldflags
BUILD_TIME := $(shell date -u '+%Y-%m-%dT%H:%M:%SZ')
LDFLAGS := -ldflags "-X main.BuildVersion=1.0.0 -X main.BuildTime=$(BUILD_TIME)"

# Default target: build the Go binary
.PHONY: build
build:
	@echo "Building $(BINARY)..."
	go build $(LDFLAGS) -o $(BINARY) ./app/main.go
	@echo "Build complete: ./$(BINARY)"

# Install the binary and systemd service (requires sudo)
.PHONY: install
install: build
	@echo "Installing $(BINARY)..."
	install -m 0755 $(BINARY) $(INSTALL_PATH)
	install -m 0644 $(SERVICE_FILE) /etc/systemd/system/
	systemctl daemon-reload
	systemctl enable $(basename $(SERVICE_FILE))
	systemctl restart $(basename $(SERVICE_FILE))
	@echo "Installation complete."

# Remove the binary and disable the service (requires sudo)
.PHONY: uninstall
uninstall:
	@echo "Uninstalling $(BINARY)..."
	-systemctl stop $(basename $(SERVICE_FILE))
	-systemctl disable $(basename $(SERVICE_FILE))
	rm -f $(INSTALL_PATH)
	rm -f /etc/systemd/system/$(SERVICE_FILE)
	systemctl daemon-reload
	@echo "Uninstallation complete."

# Check the current status of the systemd service
.PHONY: status
status:
	systemctl status $(basename $(SERVICE_FILE)) --no-pager

# View the latest service logs via journalctl
.PHONY: logs
logs:
	journalctl -u $(basename $(SERVICE_FILE)) -f

# Clean build artifacts
.PHONY: clean
clean:
	rm -f $(BINARY)
	@echo "Clean complete."
