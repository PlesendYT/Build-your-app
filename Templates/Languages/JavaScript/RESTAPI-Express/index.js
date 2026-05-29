// RESTAPI-Express — server entry point
// Configures Express with JSON parsing, user routes, health check, and error handling

import express from 'express';
import userRoutes from './routes/users.js';
import { errorHandler } from './middleware/auth.js';

const app = express();
const PORT = process.env.PORT || 3000;

// Middleware
app.use(express.json());
app.use('/api/users', userRoutes);

// Health check endpoint
app.get('/api/health', (_req, res) => {
  res.json({ status: 'ok', timestamp: new Date().toISOString() });
});

// Global error handler
app.use(errorHandler);

// Start the server
app.listen(PORT, () => {
  console.log(`Server running on http://localhost:${PORT}`);
});
