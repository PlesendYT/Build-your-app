"""SQLAlchemy ORM CRUD example with User and Post models."""

from sqlalchemy import create_engine, Column, Integer, String, Text, Boolean, ForeignKey, DateTime, func
from sqlalchemy.orm import declarative_base, relationship, Session

from config import DATABASE_URI

# Base class for all ORM models
Base = declarative_base()


class User(Base):
    """User model with a one-to-many relationship to Post."""

    __tablename__ = "users"

    id = Column(Integer, primary_key=True)
    name = Column(String(100), nullable=False)
    email = Column(String(255), unique=True, nullable=False)
    created_at = Column(DateTime, server_default=func.now())

    # ORM relationship: User has many Posts
    posts = relationship("Post", back_populates="author", cascade="all, delete-orphan")


class Post(Base):
    """Post model linked to a User via a foreign key."""

    __tablename__ = "posts"

    id = Column(Integer, primary_key=True)
    title = Column(String(200), nullable=False)
    content = Column(Text)
    published = Column(Boolean, default=False)
    author_id = Column(Integer, ForeignKey("users.id"), nullable=False)
    created_at = Column(DateTime, server_default=func.now())

    # ORM relationship: Post belongs to one User
    author = relationship("User", back_populates="posts")


def main():
    """Run CRUD operations to demonstrate SQLAlchemy usage."""
    # Create the SQLite engine and all tables
    engine = create_engine(DATABASE_URI, echo=False)
    Base.metadata.create_all(engine)

    with Session(engine) as session:
        # CREATE: Add a new user with a post
        user = User(name="Alice", email="alice@example.com")
        user.posts.append(Post(title="Hello", content="First post!", published=True))
        session.add(user)
        session.commit()
        print(f"Created user: {user.id}")

        # READ: Query all published posts with their authors
        posts = session.query(Post).filter(Post.published.is_(True)).all()
        for p in posts:
            print(f"  Post: {p.title} by {p.author.name}")

        # UPDATE: Change Alice's email
        alice = session.query(User).filter_by(email="alice@example.com").one()
        alice.email = "alice@newdomain.com"
        session.commit()
        print(f"Updated email: {alice.email}")

        # DELETE: Remove Alice (cascades to her posts)
        session.delete(alice)
        session.commit()
        print("Deleted Alice and her posts")


if __name__ == "__main__":
    main()
