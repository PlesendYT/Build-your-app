#!/bin/bash
# deploy.sh — apply all Kubernetes manifests to the current cluster
# Usage: ./deploy.sh [namespace]

set -euo pipefail  # Exit on error, undefined vars, and pipe failures

# Use the provided namespace or default to "default"
NAMESPACE="${1:-default}"

echo "--- Deploying go-service to namespace: $NAMESPACE ---"

# Apply resources in dependency order (ConfigMap first, then workloads)
echo ">>> Applying ConfigMap..."
kubectl apply -n "$NAMESPACE" -f configmap.yaml

echo ">>> Applying Deployment and HPA..."
kubectl apply -n "$NAMESPACE" -f deployment.yaml

echo ">>> Applying Service..."
kubectl apply -n "$NAMESPACE" -f service.yaml

echo ">>> Applying Ingress..."
kubectl apply -n "$NAMESPACE" -f ingress.yaml

# Wait for the deployment to roll out successfully
echo ">>> Waiting for rollout to complete..."
kubectl rollout status -n "$NAMESPACE" deployment/go-service --timeout=120s

echo "--- Deployment complete ---"
echo "Service is available at: go-service:8080"
