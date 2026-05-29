// Auth routes - register, login, and logout with JWT cookies
const express = require('express');
const router = express.Router();
const bcrypt = require('bcryptjs');
const jwt = require('jsonwebtoken');
const User = require('../models/User');

const JWT_SECRET = process.env.JWT_SECRET || 'forum-secret-key-change-in-production';

// Show registration form
router.get('/register', (req, res) => {
  res.render('register', { error: null });
});

// Handle user registration
router.post('/register', async (req, res) => {
  try {
    const { username, email, password } = req.body;
    // Check if user already exists by email
    const existing = await User.findByEmail(email);
    if (existing) {
      return res.render('register', { error: 'Email already registered' });
    }
    // Hash password and create user
    const hashedPassword = await bcrypt.hash(password, 10);
    const user = await User.create({ username, email, password: hashedPassword });
    // Generate JWT and set as cookie
    const token = jwt.sign({ id: user.id, username: user.username }, JWT_SECRET, { expiresIn: '7d' });
    res.cookie('token', token, { httpOnly: true });
    res.redirect('/threads');
  } catch (err) {
    res.render('register', { error: 'Registration failed' });
  }
});

// Show login form
router.get('/login', (req, res) => {
  res.render('login', { error: null });
});

// Handle user login
router.post('/login', async (req, res) => {
  try {
    const { email, password } = req.body;
    const user = await User.findByEmail(email);
    if (!user || !(await bcrypt.compare(password, user.password))) {
      return res.render('login', { error: 'Invalid credentials' });
    }
    const token = jwt.sign({ id: user.id, username: user.username }, JWT_SECRET, { expiresIn: '7d' });
    res.cookie('token', token, { httpOnly: true });
    res.redirect('/threads');
  } catch (err) {
    res.render('login', { error: 'Login failed' });
  }
});

// Logout by clearing the token cookie
router.get('/logout', (req, res) => {
  res.clearCookie('token');
  res.redirect('/threads');
});

module.exports = router;
