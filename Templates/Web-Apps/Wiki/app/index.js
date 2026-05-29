// Wiki app - Express server with markdown rendering and page version history
const express = require('express');
const path = require('path');
const methodOverride = require('method-override');

const app = express();

// Configure EJS view engine
app.set('view engine', 'ejs');
app.set('views', path.join(__dirname, '..', 'views'));

// Parsing and method override middleware
app.use(express.urlencoded({ extended: true }));
app.use(express.json());
app.use(methodOverride('_method'));

// Mount page routes
const pageRoutes = require('./routes/pages');
app.use('/', pageRoutes);

// Start server
const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Wiki running on port ${PORT}`));
