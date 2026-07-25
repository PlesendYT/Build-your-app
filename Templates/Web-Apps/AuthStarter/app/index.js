// Auth Starter - Express server with JWT, sessions, and OAuth2-ready auth
const express = require('express');
const path = require('path');
const cookieParser = require('cookie-parser');
const session = require('express-session');

const app = express();

// View engine setup
app.set('view engine', 'ejs');
app.set('views', path.join(__dirname, '..', 'views'));

// Middleware for parsing and sessions
app.use(express.urlencoded({ extended: true }));
app.use(express.json());
app.use(cookieParser());
app.use(session({
  secret: process.env.SESSION_SECRET || require('crypto').randomBytes(32).toString('hex'),
  resave: false,
  saveUninitialized: false,
  cookie: { secure: false }
}));

// Mount auth routes
const authRoutes = require('./routes/auth');
app.use('/auth', authRoutes);

// Mount auth middleware
const { requireAuth } = require('./middleware/auth');

// Protected dashboard route
app.get('/dashboard', requireAuth, (req, res) => {
  res.render('dashboard', { user: req.user });
});

// Home route
app.get('/', (req, res) => {
  const token = req.cookies.token;
  let user = null;
  if (token) {
    try {
      user = require('jsonwebtoken').verify(token, process.env.JWT_SECRET || 'auth-starter-jwt-secret');
    } catch (e) {}
  }
  res.render('index', { user });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Auth starter running on port ${PORT}`));
