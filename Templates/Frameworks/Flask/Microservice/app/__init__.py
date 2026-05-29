"""Flask microservice application factory."""

from flask import Flask, jsonify
import os


def create_app():
    """Create and configure the Flask microservice application."""
    app = Flask(__name__)

    @app.route('/health')
    def health():
        """Health check endpoint."""
        return jsonify({'status': 'ok'})

    @app.route('/api/v1/status')
    def status():
        """Service status endpoint with environment info."""
        return jsonify({
            'service': os.getenv('SERVICE_NAME', 'microservice'),
            'version': '1.0.0',
            'environment': os.getenv('ENV', 'development'),
        })

    return app
