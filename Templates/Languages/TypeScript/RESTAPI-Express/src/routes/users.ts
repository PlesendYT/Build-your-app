// Express router for /api/users — full CRUD endpoints with in-memory storage
import { Router, Request, Response } from 'express';

// User entity shape
interface User {
  id: number;
  name: string;
  email: string;
}

// In-memory user store seeded with sample data
const users: User[] = [
  { id: 1, name: 'Alice', email: 'alice@example.com' },
  { id: 2, name: 'Bob',   email: 'bob@example.com' },
];
let nextId = 3;

const router = Router();

// GET /api/users — list all users
router.get('/', (_req: Request, res: Response) => {
  res.json(users);
});

// GET /api/users/:id — get a single user
router.get('/:id', (req: Request, res: Response) => {
  const user = users.find(u => u.id === parseInt(req.params.id));
  if (!user) return res.status(404).json({ error: 'User not found' });
  res.json(user);
});

// POST /api/users — create a new user
router.post('/', (req: Request, res: Response) => {
  const { name, email } = req.body;
  if (!name || !email) {
    return res.status(400).json({ error: 'name and email are required' });
  }
  const user: User = { id: nextId++, name, email };
  users.push(user);
  res.status(201).json(user);
});

// PUT /api/users/:id — update an existing user
router.put('/:id', (req: Request, res: Response) => {
  const user = users.find(u => u.id === parseInt(req.params.id));
  if (!user) return res.status(404).json({ error: 'User not found' });
  const { name, email } = req.body;
  if (name)  user.name = name;
  if (email) user.email = email;
  res.json(user);
});

// DELETE /api/users/:id — remove a user
router.delete('/:id', (req: Request, res: Response) => {
  const idx = users.findIndex(u => u.id === parseInt(req.params.id));
  if (idx === -1) return res.status(404).json({ error: 'User not found' });
  users.splice(idx, 1);
  res.status(204).end();
});

export default router;
