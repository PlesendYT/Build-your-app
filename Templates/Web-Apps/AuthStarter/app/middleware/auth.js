// Auth middleware - JWT verification and route protection
const jwt = require('jsonwebtoken');
const JWT_SECRET = process.env.JWT_SECRET || 'auth-starter-jwt-secret';

// Middleware to require a valid JWT for protected routes
function requireAuth(req, res, next) {
  // Check for token in cookie or Authorization header
  const token = req.cookies.token || (req.headers.authorization && req.headers.authorization.split(' ')[1]);
  if (!token) {
    return res.redirect('/auth/login');
  }
  try {
    // Verify and decode the JWT
    const decoded = jwt.verify(token, JWT_SECRET);
    req.user = decoded;
    next();
  } catch (err) {
    res.clearCookie('token');
    res.redirect('/auth/login');
  }
}

// Middleware to optionally attach user data if logged in
function optionalAuth(req, res, next) {
  const token = req.cookies.token;
  if (token) {
    try {
      req.user = jwt.verify(token, JWT_SECRET);
    } catch (e) {
      req.user = null;
    }
  }
  next();
}

module.exports = { requireAuth, optionalAuth };
