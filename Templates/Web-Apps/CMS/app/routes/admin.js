// Admin routes - login, dashboard, CRUD for content and media uploads
const express = require('express');
const router = express.Router();
const bcrypt = require('bcryptjs');
const jwt = require('jsonwebtoken');
const Content = require('../models/Content');

const JWT_SECRET = process.env.JWT_SECRET || 'cms-secret';
const ADMIN_USER = { username: 'admin', password: bcrypt.hashSync('admin123', 10) };

// Auth middleware for admin routes
function requireAuth(req, res, next) {
  const token = req.cookies.token;
  if (!token) return res.redirect('/admin/login');
  try {
    req.user = jwt.verify(token, JWT_SECRET);
    next();
  } catch (e) {
    res.redirect('/admin/login');
  }
}

// Admin login page
router.get('/login', (req, res) => {
  res.render('admin/login', { error: null });
});

// Handle admin login
router.post('/login', async (req, res) => {
  const { username, password } = req.body;
  if (username !== ADMIN_USER.username || !bcrypt.compareSync(password, ADMIN_USER.password)) {
    return res.render('admin/login', { error: 'Invalid credentials' });
  }
  const token = jwt.sign({ username }, JWT_SECRET, { expiresIn: '1d' });
  res.cookie('token', token, { httpOnly: true });
  res.redirect('/admin/dashboard');
});

// Admin dashboard - list all content
router.get('/dashboard', requireAuth, async (req, res) => {
  const contents = await Content.findAll();
  res.render('admin/dashboard', { contents, user: req.user });
});

// Create new content form
router.get('/content/new', requireAuth, (req, res) => {
  res.render('admin/edit', { content: null, error: null });
});

// Create new content
router.post('/content', requireAuth, async (req, res) => {
  try {
    const { title, body, published } = req.body;
    if (!title || !body) {
      return res.render('admin/edit', { content: null, error: 'Title and body required' });
    }
    await Content.create({ title, body, published: published === 'on' });
    res.redirect('/admin/dashboard');
  } catch (err) {
    res.render('admin/edit', { content: null, error: 'Creation failed' });
  }
});

// Edit content form
router.get('/content/:id/edit', requireAuth, async (req, res) => {
  const content = await Content.findById(req.params.id);
  if (!content) return res.status(404).send('Content not found');
  res.render('admin/edit', { content, error: null });
});

// Update content
router.put('/content/:id', requireAuth, async (req, res) => {
  try {
    const { title, body, published } = req.body;
    await Content.update(req.params.id, { title, body, published: published === 'on' });
    res.redirect('/admin/dashboard');
  } catch (err) {
    res.render('admin/edit', { content: { id: req.params.id }, error: 'Update failed' });
  }
});

// Delete content
router.delete('/content/:id', requireAuth, async (req, res) => {
  await Content.delete(req.params.id);
  res.redirect('/admin/dashboard');
});

// Logout
router.get('/logout', (req, res) => {
  res.clearCookie('token');
  res.redirect('/admin/login');
});

module.exports = router;
