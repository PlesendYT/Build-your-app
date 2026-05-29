-- PostgreSQL schema initialization script
-- This runs automatically when the container starts

-- Create a users table with an auto-incrementing primary key
CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,          -- Auto-incrementing unique identifier
    name VARCHAR(100) NOT NULL,      -- User's display name
    email VARCHAR(255) UNIQUE NOT NULL, -- User's email (must be unique)
    created_at TIMESTAMP DEFAULT NOW()  -- Record creation timestamp
);

-- Create a posts table linked to users via foreign key
CREATE TABLE IF NOT EXISTS posts (
    id SERIAL PRIMARY KEY,
    user_id INTEGER REFERENCES users(id) ON DELETE CASCADE, -- FK to users
    title VARCHAR(200) NOT NULL,
    body TEXT,
    created_at TIMESTAMP DEFAULT NOW()
);

-- Seed data for testing right after creation
INSERT INTO users (name, email) VALUES
    ('Bob', 'bob@example.com'),
    ('Charlie', 'charlie@example.com')
ON CONFLICT (email) DO NOTHING;
