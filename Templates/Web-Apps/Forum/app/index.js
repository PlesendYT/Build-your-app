// Forum app - Express server with threads, auth, and EJS views
const express = require('express');
const path = require('path');
const cookieParser = require('cookie-parser');
const methodOverride = require('method-override');

const app = express();

// View engine setup with EJS
app.set('view engine', 'ejs');
app.set('views', path.join(__dirname, '..', 'views'));

// Middleware for parsing requests and cookies
app.use(express.urlencoded({ extended: true }));
app.use(express.json());
app.use(cookieParser());
app.use(methodOverride('_method'));

// Import route handlers
const authRoutes = require('./routes/auth');
const threadRoutes = require('./routes/threads');

// Mount routes
app.use('/auth', authRoutes);
app.use('/threads', threadRoutes);

// Home route - redirect to threads
app.get('/', (req, res) => {
  res.redirect('/threads');
});

// Start server on port 3000
const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Forum running on port ${PORT}`));
