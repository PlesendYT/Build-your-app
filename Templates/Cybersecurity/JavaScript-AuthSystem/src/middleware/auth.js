// auth.js — Express middleware for JWT verification
// Validates the Authorization header and attaches the decoded payload to req.user

const jwt = require('jsonwebtoken');

// Secret key for signing tokens (in production, load from env variables)
const SECRET_KEY = process.env.JWT_SECRET || 'change-me-in-production';

/**
 * Middleware that checks for a valid JWT Bearer token on protected routes.
 * Returns 401 if the token is missing, expired, or malformed.
 */
function authenticate(req, res, next) {
  // Extract the token from the Authorization header
  const authHeader = req.headers.authorization;
  if (!authHeader || !authHeader.startsWith('Bearer ')) {
    return res.status(401).json({ error: 'Missing or malformed Authorization header' });
  }

  const token = authHeader.split(' ')[1];

  try {
    // Verify the token and decode the payload
    const decoded = jwt.verify(token, SECRET_KEY);
    req.user = decoded;   // e.g. { id, username, iat, exp }
    next();
  } catch (err) {
    // Token expired or signature invalid
    return res.status(401).json({ error: 'Invalid or expired token' });
  }
}

module.exports = { authenticate, SECRET_KEY };
