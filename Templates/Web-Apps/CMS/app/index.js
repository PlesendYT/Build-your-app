// CMS app - Express server with content management and media upload
const express = require('express');
const path = require('path');
const cookieParser = require('cookie-parser');
const methodOverride = require('method-override');
const multer = require('multer');

const app = express();

// View engine setup
app.set('view engine', 'ejs');
app.set('views', path.join(__dirname, '..', 'views'));

// Configure multer for file uploads
const storage = multer.diskStorage({
  destination: path.join(__dirname, '..', 'public', 'uploads'),
  filename: (req, file, cb) => {
    cb(null, Date.now() + '-' + file.originalname);
  }
});
const upload = multer({ storage });

// Middleware
app.use(express.urlencoded({ extended: true }));
app.use(express.json());
app.use(cookieParser());
app.use(methodOverride('_method'));
app.use(express.static(path.join(__dirname, '..', 'public')));

// Mount routes
const contentRoutes = require('./routes/content');
const adminRoutes = require('./routes/admin');
app.use('/', contentRoutes);
app.use('/admin', adminRoutes);

// Pass upload middleware to routes via app.locals
app.locals.upload = upload;

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`CMS running on port ${PORT}`));
