// Simple Express server for CI/CD pipeline demonstration
// Returns JSON responses for testing and health checking

const express = require('express');

const app = express();
const PORT = process.env.PORT || 3000;

// Middleware to parse JSON request bodies
app.use(express.json());

// In-memory store for demonstration purposes
const items = [];

// Health check endpoint used by load balancers and orchestration tools
app.get('/health', (req, res) => {
  res.json({ status: 'ok', uptime: process.uptime() });
});

// GET /api/items - returns all stored items
app.get('/api/items', (req, res) => {
  res.json(items);
});

// POST /api/items - creates a new item
app.post('/api/items', (req, res) => {
  const { name } = req.body;
  if (!name) {
    return res.status(400).json({ error: 'Name is required' });
  }
  const item = { id: items.length + 1, name };
  items.push(item);
  res.status(201).json(item);
});

// Only start listening when this file is run directly (not during tests)
if (require.main === module) {
  app.listen(PORT, () => {
    console.log(`Server running on http://localhost:${PORT}`);
  });
}

// Export for testing purposes
module.exports = app;
