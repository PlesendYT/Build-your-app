GO ?= go

.PHONY: help build run test clean
help:
	@echo "Targets: build run test clean"
build:
	$(GO) build ./...
run:
	$(GO) run .
test:
	$(GO) test ./...
clean:
	rm -rf bin/