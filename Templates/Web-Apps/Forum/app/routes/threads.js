// Thread routes - CRUD for forum threads with posts
const express = require('express');
const router = express.Router();
const Thread = require('../models/Thread');
const User = require('../models/User');
const jwt = require('jsonwebtoken');

const JWT_SECRET = process.env.JWT_SECRET || require('crypto').randomBytes(32).toString('hex');

// Middleware to extract user from JWT cookie if present
function loadUser(req, res, next) {
  const token = req.cookies.token;
  if (token) {
    try {
      req.user = jwt.verify(token, JWT_SECRET);
    } catch (e) { req.user = null; }
  }
  next();
}

// List all threads on the index page
router.get('/', loadUser, async (req, res) => {
  const threads = await Thread.findAll();
  res.render('threads', { threads, user: req.user });
});

// Show form to create a new thread
router.get('/new', loadUser, (req, res) => {
  if (!req.user) return res.redirect('/auth/login');
  res.render('new-thread', { user: req.user, error: null });
});

// Create a new thread
router.post('/', loadUser, async (req, res) => {
  if (!req.user) return res.redirect('/auth/login');
  try {
    const { title, content } = req.body;
    if (!title || !content) {
      return res.render('new-thread', { user: req.user, error: 'Title and content required' });
    }
    await Thread.create({ title, content, userId: req.user.id, username: req.user.username });
    res.redirect('/threads');
  } catch (err) {
    res.render('new-thread', { user: req.user, error: 'Failed to create thread' });
  }
});

// View a single thread with its posts
router.get('/:id', loadUser, async (req, res) => {
  const thread = await Thread.findById(req.params.id);
  if (!thread) return res.status(404).send('Thread not found');
  const posts = await Thread.findPostsByThreadId(req.params.id);
  res.render('thread', { thread, posts, user: req.user });
});

// Add a post to a thread
router.post('/:id/posts', loadUser, async (req, res) => {
  if (!req.user) return res.redirect('/auth/login');
  const { content } = req.body;
  if (content) {
    await Thread.addPost({ threadId: req.params.id, content, userId: req.user.id, username: req.user.username });
  }
  res.redirect(`/threads/${req.params.id}`);
});

module.exports = router;
