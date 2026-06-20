"""Flask REST API template."""
from __future__ import annotations

from flask import Flask, request
from flask_restful import Api, Resource
from flask_sqlalchemy import SQLAlchemy

app = Flask(__name__)
app.config["SQLALCHEMY_DATABASE_URI"] = "sqlite:///app.db"
app.config["SQLALCHEMY_TRACK_MODIFICATIONS"] = False
db = SQLAlchemy(app)
api = Api(app)


class Item(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String(120), nullable=False)

    def to_dict(self) -> dict:
        return {"id": self.id, "name": self.name}


class ItemList(Resource):
    def get(self) -> list[dict]:
        return [i.to_dict() for i in Item.query.all()]

    def post(self) -> tuple[dict, int]:
        data = request.get_json(silent=True) or {}
        item = Item(name=str(data.get("name", "")).strip() or "unnamed")
        db.session.add(item)
        db.session.commit()
        return item.to_dict(), 201


api.add_resource(ItemList, "/items")


def main() -> None:
    with app.app_context():
        db.create_all()
    app.run(debug=True)


if __name__ == "__main__":
    main()