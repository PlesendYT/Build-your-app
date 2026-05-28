from flask_restful import Resource, reqparse
from flask import jsonify

parser = reqparse.RequestParser()
parser.add_argument('title', type=str, required=True)
parser.add_argument('description', type=str, required=True)

items = []

class ItemListResource(Resource):
    def get(self):
        return jsonify(items)

    def post(self):
        args = parser.parse_args()
        item = {
            'id': len(items) + 1,
            'title': args['title'],
            'description': args['description'],
        }
        items.append(item)
        return item, 201

class ItemResource(Resource):
    def get(self, item_id):
        item = next((i for i in items if i['id'] == item_id), None)
        if item:
            return item
        return {'message': 'Item not found'}, 404

    def delete(self, item_id):
        global items
        items = [i for i in items if i['id'] != item_id]
        return '', 204
