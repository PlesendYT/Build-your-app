// User model - in-memory data store for forum users
// Replace this with a database adapter (e.g., PostgreSQL, MongoDB) in production
const users = [];
let nextId = 1;

const User = {
  // Find a user by their email address
  findByEmail(email) {
    return users.find(u => u.email === email) || null;
  },

  // Find a user by their unique ID
  findById(id) {
    return users.find(u => u.id === id) || null;
  },

  // Create a new user with the given details
  create({ username, email, password }) {
    const user = { id: nextId++, username, email, password, createdAt: new Date() };
    users.push(user);
    return user;
  }
};

module.exports = User;
