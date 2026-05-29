"""Flask REST API application factory."""

from flask import Flask
from flask_restful import Api
from flask_cors import CORS
from .resources import ItemResource, ItemListResource


def create_app():
    """Create and configure the Flask REST API application."""
    app = Flask(__name__)
    CORS(app)
    api = Api(app)

    # Register API endpoints
    api.add_resource(ItemListResource, '/api/items')
    api.add_resource(ItemResource, '/api/items/<int:item_id>')

    return app
