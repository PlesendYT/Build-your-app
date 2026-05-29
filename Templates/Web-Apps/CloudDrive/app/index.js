// Cloud Drive app - Express server with folder structure and file management
const express = require('express');
const path = require('path');
const multer = require('multer');
const { nanoid } = require('nanoid');

const app = express();

// Configure multer for file uploads
const storage = multer.diskStorage({
  destination: path.join(__dirname, '..', 'storage'),
  filename: (req, file, cb) => {
    cb(null, nanoid(12) + path.extname(file.originalname));
  }
});
const upload = multer({ storage });

// Middleware
app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use(express.static(path.join(__dirname, '..', 'public')));

// Mount drive routes
const driveRoutes = require('./routes/drive');
app.use('/api/drive', upload.single('file'), driveRoutes);

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Cloud drive running on port ${PORT}`));
