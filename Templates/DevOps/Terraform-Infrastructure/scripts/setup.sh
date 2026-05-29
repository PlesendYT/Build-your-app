#!/bin/bash
# setup.sh — Initialise and apply Terraform infrastructure
# Usage: ./scripts/setup.sh [apply|plan|destroy]

set -euo pipefail

# Change to the project root directory
SCRIPT_DIR="$(cd "$(dirname "$0")/.." && pwd)"
cd "$SCRIPT_DIR"

# Assign the command from the first argument, defaulting to "apply"
COMMAND="${1:-apply}"

echo "=== Terraform Infrastructure Setup ==="
echo "Command: $COMMAND"

# Initialise Terraform with backend and provider plugins
echo ">>> Initialising Terraform..."
terraform init -upgrade

# Validate the configuration files for syntax correctness
echo ">>> Validating configuration..."
terraform validate

# Execute the requested command (plan, apply, or destroy)
case "$COMMAND" in
  plan)
    echo ">>> Generating execution plan..."
    terraform plan -out=tfplan
    ;;
  apply)
    echo ">>> Applying infrastructure changes..."
    terraform apply -auto-approve
    ;;
  destroy)
    echo ">>> Destroying all managed infrastructure..."
    terraform destroy -auto-approve
    ;;
  *)
    echo "Usage: $0 [plan|apply|destroy]"
    exit 1
    ;;
esac

echo "=== Done ==="
