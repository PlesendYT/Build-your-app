# Database configuration for SQLAlchemy
# Uses SQLite for local development; swap to PostgreSQL in production

import os

# Base directory of the application
BASE_DIR = os.path.dirname(os.path.abspath(__file__))

# SQLAlchemy database URI (SQLite by default)
DATABASE_URI = os.getenv("DATABASE_URL", f"sqlite:///{os.path.join(BASE_DIR, 'app.db')}")

# Echo SQL statements to stdout (useful for debugging)
SQLALCHEMY_ECHO = True
