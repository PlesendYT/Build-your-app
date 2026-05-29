// user.js — In-memory user model with password hashing
// Provides find, create, and verify operations using bcryptjs

const bcrypt = require('bcryptjs');

// In-memory store (replace with a database in production)
const users = [];

/**
 * Create a new user with a hashed password.
 * Returns the created user object without the password field.
 */
async function createUser(username, password) {
  // Hash the password with a salt round of 10
  const hashedPassword = await bcrypt.hash(password, 10);
  const user = {
    id: users.length + 1,
    username,
    password: hashedPassword,
    createdAt: new Date().toISOString(),
  };
  users.push(user);

  // Return user data without the password hash
  const { password: _, ...safeUser } = user;
  return safeUser;
}

/**
 * Find a user by username.
 * Returns the full user object (including password hash) or null.
 */
function findByUsername(username) {
  return users.find(u => u.username === username) || null;
}

/**
 * Find a user by their unique id.
 * Returns user data without the password hash.
 */
function findById(id) {
  const user = users.find(u => u.id === id);
  if (!user) return null;
  const { password: _, ...safeUser } = user;
  return safeUser;
}

/**
 * Verify a plaintext password against the stored hash.
 */
async function verifyPassword(plainPassword, hashedPassword) {
  return bcrypt.compare(plainPassword, hashedPassword);
}

module.exports = { createUser, findByUsername, findById, verifyPassword };
