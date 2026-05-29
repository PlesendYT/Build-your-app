// Drive routes - folder navigation, file CRUD, and sharing
const express = require('express');
const router = express.Router();
const path = require('path');
const { nanoid } = require('nanoid');
const Folder = require('../models/Folder');
const File = require('../models/File');

// Get contents of a folder (files and subfolders)
router.get('/folder/:id', (req, res) => {
  const folderId = Number(req.params.id) || null;
  const folder = folderId ? Folder.findById(folderId) : null;
  if (req.params.id !== 'root' && !folder) return res.status(404).json({ error: 'Folder not found' });
  const folders = Folder.findByParentId(folderId);
  const files = File.findByFolderId(folderId);
  res.json({ folder, folders, files, breadcrumb: Folder.getBreadcrumb(folderId) });
});

// Create a new folder
router.post('/folder', (req, res) => {
  const { name, parentId } = req.body;
  if (!name) return res.status(400).json({ error: 'Folder name required' });
  const folder = Folder.create({ name, parentId: parentId ? Number(parentId) : null });
  res.status(201).json(folder);
});

// Upload a file to a folder
router.post('/upload', (req, res) => {
  if (!req.file) return res.status(400).json({ error: 'No file uploaded' });
  const folderId = req.body.folderId ? Number(req.body.folderId) : null;
  const shareCode = nanoid(10);
  const file = File.create({
    originalName: req.file.originalname,
    storedName: req.file.filename,
    size: req.file.size,
    mimeType: req.file.mimetype,
    folderId,
    shareCode
  });
  res.status(201).json(file);
});

// Get a share link for a file
router.get('/file/:id/share', (req, res) => {
  const file = File.findById(req.params.id);
  if (!file) return res.status(404).json({ error: 'File not found' });
  res.json({ shareCode: file.shareCode, downloadUrl: `/api/drive/download/${file.shareCode}` });
});

// Download a file by share code
router.get('/download/:code', (req, res) => {
  const file = File.findByShareCode(req.params.code);
  if (!file) return res.status(404).json({ error: 'File not found' });
  File.incrementDownloads(file.id);
  const filePath = path.join(__dirname, '..', '..', 'storage', file.storedName);
  res.download(filePath, file.originalName);
});

// Delete a file
router.delete('/file/:id', (req, res) => {
  File.delete(req.params.id);
  res.status(204).end();
});

// Delete a folder (and contents recursively)
router.delete('/folder/:id', (req, res) => {
  Folder.delete(req.params.id);
  File.deleteByFolderId(req.params.id);
  res.status(204).end();
});

module.exports = router;
