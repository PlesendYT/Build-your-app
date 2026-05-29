// Kanban app - Express API server for board, columns, and card management
const express = require('express');
const path = require('path');

const app = express();

// Middleware for JSON parsing
app.use(express.json());

// Serve static frontend files
app.use(express.static(path.join(__dirname, '..', 'public')));

// Mount board routes
const boardRoutes = require('./routes/boards');
app.use('/api/boards', boardRoutes);

// Serve the main HTML page
app.get('/', (req, res) => {
  res.sendFile(path.join(__dirname, '..', 'public', 'index.html'));
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Kanban board running on port ${PORT}`));
