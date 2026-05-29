// Express server entry point — sets up JSON parsing, routes, health check, and error handling
import express, { Request, Response, NextFunction } from 'express';
import userRoutes from './routes/users.js';

const app = express();
const PORT = process.env.PORT || 3000;

// JSON body parser middleware
app.use(express.json());
// Mount user CRUD routes
app.use('/api/users', userRoutes);

// Health check endpoint
app.get('/api/health', (_req: Request, res: Response) => {
  res.json({ status: 'ok', timestamp: new Date().toISOString() });
});

// Global error handler
app.use((err: Error, _req: Request, res: Response, _next: NextFunction) => {
  console.error(err.stack);
  res.status(500).json({ error: err.message || 'Internal Server Error' });
});

// Start the server
app.listen(PORT, () => {
  console.log(`Server running on http://localhost:${PORT}`);
});
