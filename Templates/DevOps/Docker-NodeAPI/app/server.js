// Simple Express API server for the Docker-NodeAPI template
// Demonstrates a multi-stage Docker build with a MongoDB backend

const express = require('express');
const mongoose = require('mongoose');

// Read configuration from environment variables with sensible defaults
const PORT = process.env.PORT || 3000;
const MONGO_URI = process.env.MONGO_URI || 'mongodb://localhost:27017/mydb';

// Create the Express application instance
const app = express();

// Parse incoming JSON request bodies
app.use(express.json());

// ----- In-memory items array (fallback when MongoDB is unavailable) -----
const items = [];

// Health check endpoint used by orchestration tools
app.get('/health', (req, res) => {
  res.json({ status: 'ok', timestamp: new Date().toISOString() });
});

// GET /api/items - return the full list of items
app.get('/api/items', (req, res) => {
  res.json(items);
});

// POST /api/items - add a new item to the list
app.post('/api/items', (req, res) => {
  const { name } = req.body;
  if (!name) {
    return res.status(400).json({ error: 'name is required' });
  }
  const item = { id: items.length + 1, name };
  items.push(item);
  res.status(201).json(item);
});

// Connect to MongoDB and start the HTTP server
mongoose.connect(MONGO_URI)
  .then(() => {
    console.log('Connected to MongoDB');
    app.listen(PORT, () => {
      console.log(`Server running on port ${PORT}`);
    });
  })
  .catch((err) => {
    // Start server even if MongoDB is not available (fallback to in-memory)
    console.warn('MongoDB connection failed, using in-memory storage:', err.message);
    app.listen(PORT, () => {
      console.log(`Server running on port ${PORT} (no database)`);
    });
  });
