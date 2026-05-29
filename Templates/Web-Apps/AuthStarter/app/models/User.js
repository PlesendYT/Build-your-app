// User model - in-memory store with verification and password reset support
const users = [];
let nextId = 1;

const User = {
  // Find user by email
  findByEmail(email) {
    return users.find(u => u.email === email) || null;
  },

  // Find user by ID
  findById(id) {
    return users.find(u => u.id === id) || null;
  },

  // Create a new user with verification token
  create({ username, email, password, verifyToken }) {
    const user = { id: nextId++, username, email, password, verified: false, verifyToken, resetToken: null, createdAt: new Date() };
    users.push(user);
    return user;
  },

  // Mark a user's email as verified
  verifyEmail(email) {
    const user = users.find(u => u.email === email);
    if (user) {
      user.verified = true;
      user.verifyToken = null;
    }
    return user;
  },

  // Store a password reset token
  setResetToken(email, token) {
    const user = users.find(u => u.email === email);
    if (user) user.resetToken = token;
  },

  // Update user's password (clear reset token)
  updatePassword(email, hashedPassword) {
    const user = users.find(u => u.email === email);
    if (user) {
      user.password = hashedPassword;
      user.resetToken = null;
    }
    return user;
  }
};

module.exports = User;
