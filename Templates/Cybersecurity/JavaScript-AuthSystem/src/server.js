// server.js — Express server with JWT register/login/verify endpoints
// Serves a static login form on GET / and exposes a /api/auth routes

const express = require('express');
const jwt = require('jsonwebtoken');
const path = require('path');
const { authenticate, SECRET_KEY } = require('./middleware/auth');
const { createUser, findByUsername, verifyPassword } = require('./models/user');

const app = express();
const PORT = process.env.PORT || 3000;

// Parse JSON and URL-encoded request bodies
app.use(express.json());
app.use(express.urlencoded({ extended: true }));
// Serve the static login form from the public directory
app.use(express.static(path.join(__dirname, '..', 'public')));

// POST /api/auth/register — Create a new user account
app.post('/api/auth/register', async (req, res) => {
  const { username, password } = req.body;
  if (!username || !password) {
    return res.status(400).json({ error: 'Username and password are required' });
  }
  if (findByUsername(username)) {
    return res.status(409).json({ error: 'Username already exists' });
  }
  const user = await createUser(username, password);
  // Issue a JWT that expires in 24 hours
  const token = jwt.sign({ id: user.id, username: user.username }, SECRET_KEY, {
    expiresIn: '24h',
  });
  res.status(201).json({ user, token });
});

// POST /api/auth/login — Authenticate and receive a JWT
app.post('/api/auth/login', async (req, res) => {
  const { username, password } = req.body;
  if (!username || !password) {
    return res.status(400).json({ error: 'Username and password are required' });
  }
  const user = findByUsername(username);
  if (!user || !(await verifyPassword(password, user.password))) {
    return res.status(401).json({ error: 'Invalid credentials' });
  }
  const token = jwt.sign({ id: user.id, username: user.username }, SECRET_KEY, {
    expiresIn: '24h',
  });
  const { password: _, ...safeUser } = user;
  res.json({ user: safeUser, token });
});

// GET /api/auth/verify — Verify a token (protected route)
app.get('/api/auth/verify', authenticate, (req, res) => {
  res.json({ valid: true, user: req.user });
});

app.listen(PORT, () => {
  console.log(`Auth server running on http://localhost:${PORT}`);
});
