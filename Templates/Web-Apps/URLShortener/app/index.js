// URL Shortener - Express server with short code generation and click tracking
const express = require('express');
const path = require('path');
const { nanoid } = require('nanoid');
const Link = require('./models/Link');

const app = express();

// Middleware for parsing
app.use(express.urlencoded({ extended: true }));
app.use(express.json());

// Serve static frontend
app.use(express.static(path.join(__dirname, '..', 'public')));

// API: Create a shortened URL
app.post('/api/shorten', async (req, res) => {
  try {
    const { url } = req.body;
    if (!url) return res.status(400).json({ error: 'URL is required' });
    // Generate a unique short code
    const code = nanoid(7);
    const link = await Link.create({ url, code });
    res.json({ shortUrl: `${req.protocol}://${req.get('host')}/${code}`, code: link.code });
  } catch (err) {
    res.status(500).json({ error: 'Failed to shorten URL' });
  }
});

// API: Get link stats (click count)
app.get('/api/stats/:code', async (req, res) => {
  const link = await Link.findByCode(req.params.code);
  if (!link) return res.status(404).json({ error: 'Link not found' });
  res.json({ url: link.url, code: link.code, clicks: link.clicks, createdAt: link.createdAt });
});

// Redirect short code to original URL (with click tracking)
app.get('/:code', async (req, res) => {
  const link = await Link.findByCode(req.params.code);
  if (!link) return res.status(404).send('Link not found');
  // Increment the click counter
  await Link.incrementClicks(req.params.code);
  res.redirect(301, link.url);
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`URL Shortener running on port ${PORT}`));
