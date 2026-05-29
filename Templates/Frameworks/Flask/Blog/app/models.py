"""Database models for the Flask blog application."""

from .extensions import db


class Post(db.Model):
    """Blog post model with title, content, and timestamp."""

    id = db.Column(db.Integer, primary_key=True)
    title = db.Column(db.String(200), nullable=False)
    content = db.Column(db.Text, nullable=False)
    created_at = db.Column(db.DateTime, server_default=db.func.now())


class Comment(db.Model):
    """Comment model linked to a blog post."""

    id = db.Column(db.Integer, primary_key=True)
    post_id = db.Column(db.Integer, db.ForeignKey('post.id'))
    author = db.Column(db.String(100))
    text = db.Column(db.Text)
    post = db.relationship('Post', backref='comments')
