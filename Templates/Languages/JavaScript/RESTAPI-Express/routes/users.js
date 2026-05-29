// User routes — CRUD operations for the users resource
// Implements GET, POST, PUT, DELETE with in-memory storage

import { Router } from 'express';

const router = Router();

// In-memory user data
const users = [
  { id: 1, name: 'Alice', email: 'alice@example.com' },
  { id: 2, name: 'Bob',   email: 'bob@example.com' },
];
let nextId = 3;

// GET / — list all users
router.get('/', (_req, res) => {
  res.json(users);
});

// GET /:id — get a single user by ID
router.get('/:id', (req, res) => {
  const user = users.find(u => u.id === parseInt(req.params.id));
  if (!user) return res.status(404).json({ error: 'User not found' });
  res.json(user);
});

// POST / — create a new user (requires name and email)
router.post('/', (req, res) => {
  const { name, email } = req.body;
  if (!name || !email) {
    return res.status(400).json({ error: 'name and email are required' });
  }
  const user = { id: nextId++, name, email };
  users.push(user);
  res.status(201).json(user);
});

// PUT /:id — update an existing user's name and/or email
router.put('/:id', (req, res) => {
  const user = users.find(u => u.id === parseInt(req.params.id));
  if (!user) return res.status(404).json({ error: 'User not found' });
  const { name, email } = req.body;
  if (name)  user.name = name;
  if (email) user.email = email;
  res.json(user);
});

// DELETE /:id — delete a user by ID
router.delete('/:id', (req, res) => {
  const idx = users.findIndex(u => u.id === parseInt(req.params.id));
  if (idx === -1) return res.status(404).json({ error: 'User not found' });
  users.splice(idx, 1);
  res.status(204).end();
});

export default router;
