// RESTAPI-Fastify — server entry point
// Configures Fastify with CORS, health check, and user routes

import Fastify from 'fastify';
import cors from '@fastify/cors';
import userRoutes from './routes/users.js';

const app = Fastify({ logger: true });
const PORT = process.env.PORT || 3000;

// Register plugins and route modules
await app.register(cors);
await app.register(userRoutes, { prefix: '/api/users' });

// Health check endpoint
app.get('/api/health', async () => {
  return { status: 'ok', timestamp: new Date().toISOString() };
});

/** Starts the Fastify server on the configured port */
async function start() {
  try {
    await app.listen({ port: PORT });
    console.log(`Server running on http://localhost:${PORT}`);
  } catch (err) {
    app.log.error(err);
    process.exit(1);
  }
}

start();
