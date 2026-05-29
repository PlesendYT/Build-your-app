// Paste routes - create, view, and list pastes with syntax highlighting
const express = require('express');
const router = express.Router();
const hljs = require('highlight.js');
const { nanoid } = require('nanoid');
const Paste = require('../models/Paste');

// Home page - show create form and recent pastes
router.get('/', (req, res) => {
  const recent = Paste.findRecent();
  res.render('index', { recent, error: null });
});

// Create a new paste
router.post('/', (req, res) => {
  try {
    const { title, content, language, expiration } = req.body;
    if (!content) {
      return res.render('index', { recent: Paste.findRecent(), error: 'Content is required' });
    }
    // Calculate expiration date if set
    let expiresAt = null;
    if (expiration && expiration !== 'never') {
      const hours = parseInt(expiration);
      expiresAt = new Date(Date.now() + hours * 60 * 60 * 1000);
    }
    const paste = Paste.create({ title: title || 'Untitled', content, language: language || 'plaintext', expiresAt });
    res.redirect(`/${paste.code}`);
  } catch (err) {
    res.render('index', { recent: Paste.findRecent(), error: 'Failed to create paste' });
  }
});

// View a single paste by its short code
router.get('/:code', (req, res) => {
  const paste = Paste.findByCode(req.params.code);
  if (!paste) return res.status(404).send('Paste not found or expired');
  // Check if paste has expired
  if (paste.expiresAt && new Date() > new Date(paste.expiresAt)) {
    Paste.delete(paste.code);
    return res.status(404).send('Paste has expired');
  }
  // Highlight the code using highlight.js
  let highlighted;
  try {
    highlighted = hljs.highlight(paste.content, { language: paste.language || 'plaintext' }).value;
  } catch (e) {
    highlighted = hljs.highlightAuto(paste.content).value;
  }
  res.render('paste', { paste, highlighted });
});

// Raw text view (no highlighting)
router.get('/:code/raw', (req, res) => {
  const paste = Paste.findByCode(req.params.code);
  if (!paste) return res.status(404).send('Paste not found');
  res.set('Content-Type', 'text/plain');
  res.send(paste.content);
});

module.exports = router;
