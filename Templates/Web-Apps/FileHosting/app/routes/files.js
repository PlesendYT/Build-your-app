// File routes - upload, list, download, and delete files with share links
const express = require('express');
const router = express.Router();
const path = require('path');
const { nanoid } = require('nanoid');
const File = require('../models/File');

// Upload a new file
router.post('/upload', async (req, res) => {
  try {
    if (!req.file) return res.status(400).json({ error: 'No file uploaded' });
    // Create file record with a unique share code
    const shareCode = nanoid(10);
    const file = await File.create({
      originalName: req.file.originalname,
      storedName: req.file.filename,
      size: req.file.size,
      mimeType: req.file.mimetype,
      shareCode
    });
    res.json({ file, downloadUrl: `/api/files/download/${shareCode}` });
  } catch (err) {
    res.status(500).json({ error: 'Upload failed' });
  }
});

// List all uploaded files
router.get('/', async (req, res) => {
  const files = await File.findAll();
  res.json(files);
});

// Download a file by its share code
router.get('/download/:code', async (req, res) => {
  const file = await File.findByShareCode(req.params.code);
  if (!file) return res.status(404).json({ error: 'File not found' });
  // Increment download counter
  await File.incrementDownloads(file.id);
  const filePath = path.join(__dirname, '..', '..', 'uploads', file.storedName);
  res.download(filePath, file.originalName);
});

// Delete a file by ID
router.delete('/:id', async (req, res) => {
  const deleted = await File.delete(req.params.id);
  if (!deleted) return res.status(404).json({ error: 'File not found' });
  res.status(204).end();
});

module.exports = router;
