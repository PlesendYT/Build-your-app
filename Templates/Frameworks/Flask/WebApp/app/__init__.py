"""Flask WebApp application factory."""

from flask import Flask
from .extensions import db
from .routes import main


def create_app():
    """Create and configure the Flask WebApp application."""
    app = Flask(__name__)
    app.config['SECRET_KEY'] = 'change-this-secret'
    app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///data.db'

    # Initialize extensions and register blueprints
    db.init_app(app)
    app.register_blueprint(main)

    # Create database tables within application context
    with app.app_context():
        db.create_all()

    return app
