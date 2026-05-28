from flask import Flask, jsonify
import os

def create_app():
    app = Flask(__name__)

    @app.route('/health')
    def health():
        return jsonify({'status': 'ok'})

    @app.route('/api/v1/status')
    def status():
        return jsonify({
            'service': os.getenv('SERVICE_NAME', 'microservice'),
            'version': '1.0.0',
            'environment': os.getenv('ENV', 'development'),
        })

    return app
