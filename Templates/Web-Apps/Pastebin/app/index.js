// Pastebin app - Express server with syntax highlighting and expiration
const express = require('express');
const path = require('path');
const { nanoid } = require('nanoid');
const hljs = require('highlight.js');
const Paste = require('./models/Paste');

const app = express();

// View engine setup
app.set('view engine', 'ejs');
app.set('views', path.join(__dirname, '..', 'views'));

// Middleware
app.use(express.urlencoded({ extended: true }));
app.use(express.json());

// Mount paste routes
const pasteRoutes = require('./routes/pastes');
app.use('/', pasteRoutes);

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Pastebin running on port ${PORT}`));
