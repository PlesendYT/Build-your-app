// Page routes - CRUD for wiki pages with markdown and version history
const express = require('express');
const router = express.Router();
const { marked } = require('marked');
const Page = require('../models/Page');

// Home page - list all wiki pages
router.get('/', async (req, res) => {
  const pages = await Page.findAll();
  res.render('index', { pages });
});

// Show the create page form
router.get('/new', (req, res) => {
  res.render('edit', { page: null, error: null });
});

// Create a new wiki page
router.post('/', async (req, res) => {
  try {
    const { title, content } = req.body;
    if (!title || !content) {
      return res.render('edit', { page: null, error: 'Title and content required' });
    }
    const page = await Page.create({ title, content });
    res.redirect(`/${page.slug}`);
  } catch (err) {
    res.render('edit', { page: null, error: 'Failed to create page' });
  }
});

// View a wiki page by slug
router.get('/:slug', async (req, res) => {
  const page = await Page.findBySlug(req.params.slug);
  if (!page) return res.status(404).render('edit', { page: null, error: 'Page not found' });
  const html = marked(page.content);
  res.render('page', { page, html });
});

// Show the edit form for an existing page
router.get('/:slug/edit', async (req, res) => {
  const page = await Page.findBySlug(req.params.slug);
  if (!page) return res.status(404).send('Page not found');
  res.render('edit', { page, error: null });
});

// Update an existing page (saves a new version)
router.put('/:slug', async (req, res) => {
  try {
    const { content } = req.body;
    await Page.update(req.params.slug, content);
    res.redirect(`/${req.params.slug}`);
  } catch (err) {
    res.render('edit', { page: { slug: req.params.slug }, error: 'Update failed' });
  }
});

// View version history for a page
router.get('/:slug/history', async (req, res) => {
  const page = await Page.findBySlug(req.params.slug);
  if (!page) return res.status(404).send('Page not found');
  const history = await Page.getHistory(req.params.slug);
  res.render('history', { page, history });
});

module.exports = router;
