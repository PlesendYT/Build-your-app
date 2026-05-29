// Paste model - in-memory store for pastes with expiration support
const pastes = [];
let nextId = 1;

const Paste = {
  // Get the 10 most recent non-expired pastes
  findRecent() {
    const now = new Date();
    return pastes
      .filter(p => !p.expiresAt || new Date(p.expiresAt) > now)
      .reverse()
      .slice(0, 10);
  },

  // Find a paste by its short code
  findByCode(code) {
    return pastes.find(p => p.code === code) || null;
  },

  // Create a new paste with a unique code
  create({ title, content, language, expiresAt }) {
    const code = require('nanoid').nanoid(8);
    const paste = { id: nextId++, code, title, content, language: language || 'plaintext', expiresAt: expiresAt || null, createdAt: new Date() };
    pastes.push(paste);
    return paste;
  },

  // Delete a paste by code
  delete(code) {
    const idx = pastes.findIndex(p => p.code === code);
    if (idx !== -1) pastes.splice(idx, 1);
  }
};

module.exports = Paste;
