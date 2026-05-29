// File hosting app - Express server for file uploads and share links
const express = require('express');
const path = require('path');
const multer = require('multer');
const { nanoid } = require('nanoid');
const File = require('./models/File');

const app = express();

// Configure multer for file uploads to the uploads directory
const storage = multer.diskStorage({
  destination: path.join(__dirname, '..', 'uploads'),
  filename: (req, file, cb) => {
    // Generate a unique filename to prevent collisions
    const ext = path.extname(file.originalname);
    cb(null, nanoid(10) + ext);
  }
});
const upload = multer({ storage });

// Middleware
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// Mount file routes
const fileRoutes = require('./routes/files');
app.use('/api/files', upload.single('file'), fileRoutes);

// Serve static frontend
app.use(express.static(path.join(__dirname, '..', 'public')));

// Serve uploaded files
app.use('/uploads', express.static(path.join(__dirname, '..', 'uploads')));

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`File hosting running on port ${PORT}`));
