// Auth routes - register, login, verify email, forgot password
const express = require('express');
const router = express.Router();
const bcrypt = require('bcryptjs');
const jwt = require('jsonwebtoken');
const User = require('../models/User');

const JWT_SECRET = process.env.JWT_SECRET || 'auth-starter-jwt-secret';

// Show registration form
router.get('/register', (req, res) => {
  res.render('register', { error: null });
});

// Handle user registration
router.post('/register', async (req, res) => {
  try {
    const { username, email, password } = req.body;
    // Check if email is already taken
    const existing = await User.findByEmail(email);
    if (existing) {
      return res.render('register', { error: 'Email already registered' });
    }
    // Hash password and create user with verification token
    const hashedPassword = await bcrypt.hash(password, 10);
    const verifyToken = jwt.sign({ email }, JWT_SECRET, { expiresIn: '24h' });
    const user = await User.create({ username, email, password: hashedPassword, verifyToken });
    res.render('login', { error: 'Account created! Please check your email to verify.' });
  } catch (err) {
    res.render('register', { error: 'Registration failed' });
  }
});

// Verify email address
router.get('/verify/:token', async (req, res) => {
  try {
    const { email } = jwt.verify(req.params.token, JWT_SECRET);
    await User.verifyEmail(email);
    res.render('login', { error: 'Email verified! You can now login.' });
  } catch (err) {
    res.status(400).send('Invalid or expired verification token');
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
      return res.render('login', { error: 'Invalid email or password' });
    }
    if (!user.verified) {
      return res.render('login', { error: 'Please verify your email first' });
    }
    // Generate JWT and set as HTTP-only cookie
    const token = jwt.sign({ id: user.id, username: user.username, email: user.email }, JWT_SECRET, { expiresIn: '7d' });
    res.cookie('token', token, { httpOnly: true, maxAge: 7 * 24 * 60 * 60 * 1000 });
    res.redirect('/dashboard');
  } catch (err) {
    res.render('login', { error: 'Login failed' });
  }
});

// Show forgot password form
router.get('/forgot', (req, res) => {
  res.render('forgot', { error: null, message: null });
});

// Handle forgot password (generate reset token)
router.post('/forgot', async (req, res) => {
  const { email } = req.body;
  const user = await User.findByEmail(email);
  if (user) {
    const resetToken = jwt.sign({ email }, JWT_SECRET, { expiresIn: '1h' });
    await User.setResetToken(email, resetToken);
    // In production, send this token via email
    console.log(`Password reset token for ${email}: ${resetToken}`);
  }
  // Always show the same message to prevent email enumeration
  res.render('forgot', { error: null, message: 'If the email exists, a reset link has been sent.' });
});

// Show reset password form
router.get('/reset/:token', async (req, res) => {
  try {
    const { email } = jwt.verify(req.params.token, JWT_SECRET);
    const user = await User.findByEmail(email);
    if (!user || user.resetToken !== req.params.token) {
      return res.status(400).send('Invalid or expired reset token');
    }
    res.render('reset', { token: req.params.token, error: null });
  } catch (err) {
    res.status(400).send('Invalid or expired reset token');
  }
});

// Handle password reset
router.post('/reset/:token', async (req, res) => {
  try {
    const { email } = jwt.verify(req.params.token, JWT_SECRET);
    const { password } = req.body;
    const hashedPassword = await bcrypt.hash(password, 10);
    await User.updatePassword(email, hashedPassword);
    res.render('login', { error: 'Password reset successful! Please login.' });
  } catch (err) {
    res.status(400).send('Reset failed');
  }
});

// Logout
router.get('/logout', (req, res) => {
  res.clearCookie('token');
  req.session.destroy();
  res.redirect('/');
});

module.exports = router;
