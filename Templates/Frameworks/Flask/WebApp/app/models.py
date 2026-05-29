"""Database models for the Flask WebApp."""

from .extensions import db


class Item(db.Model):
    """Item model with title, description, and creation timestamp."""

    id = db.Column(db.Integer, primary_key=True)
    title = db.Column(db.String(200), nullable=False)
    description = db.Column(db.Text, nullable=False)
    created_at = db.Column(db.DateTime, server_default=db.func.now())

    def save(self):
        """Persist the item to the database."""
        db.session.add(self)
        db.session.commit()

    def __repr__(self):
        return f'<Item {self.title}>'
