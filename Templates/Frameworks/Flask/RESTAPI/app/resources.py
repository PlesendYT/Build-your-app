"""API resources for the Flask REST API."""

from flask_restful import Resource, reqparse
from flask import jsonify

# Request parser for item creation
parser = reqparse.RequestParser()
parser.add_argument('title', type=str, required=True)
parser.add_argument('description', type=str, required=True)

# In-memory item storage (replace with a database in production)
items = []


class ItemListResource(Resource):
    """Resource for listing and creating items."""

    def get(self):
        """Return the full list of items."""
        return jsonify(items)

    def post(self):
        """Create a new item from parsed arguments."""
        args = parser.parse_args()
        item = {
            'id': len(items) + 1,
            'title': args['title'],
            'description': args['description'],
        }
        items.append(item)
        return item, 201


class ItemResource(Resource):
    """Resource for retrieving and deleting individual items."""

    def get(self, item_id):
        """Return a single item by its ID."""
        item = next((i for i in items if i['id'] == item_id), None)
        if item:
            return item
        return {'message': 'Item not found'}, 404

    def delete(self, item_id):
        """Delete an item by its ID."""
        global items
        items = [i for i in items if i['id'] != item_id]
        return '', 204
