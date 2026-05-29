// Content routes - public-facing pages and content display
const express = require('express');
const router = express.Router();
const Content = require('../models/Content');

// Home page - list all published content
router.get('/', async (req, res) => {
  const contents = await Content.findAllPublished();
  res.render('public/index', { contents });
});

// View a single content item by slug
router.get('/:slug', async (req, res) => {
  const content = await Content.findBySlug(req.params.slug);
  if (!content) return res.status(404).send('Content not found');
  res.render('public/view', { content });
});

module.exports = router;
