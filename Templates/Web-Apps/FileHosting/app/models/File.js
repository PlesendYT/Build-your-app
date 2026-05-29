// File model - in-memory store for uploaded file metadata
const files = [];
let nextId = 1;

const File = {
  // Get all files ordered by upload date descending
  findAll() {
    return [...files].reverse();
  },

  // Find a file by its share code
  findByShareCode(code) {
    return files.find(f => f.shareCode === code) || null;
  },

  // Create a new file entry after upload
  create({ originalName, storedName, size, mimeType, shareCode }) {
    const file = { id: nextId++, originalName, storedName, size, mimeType, shareCode, downloads: 0, createdAt: new Date() };
    files.push(file);
    return file;
  },

  // Increment the download counter for a file
  incrementDownloads(id) {
    const file = files.find(f => f.id === Number(id));
    if (file) file.downloads++;
  },

  // Delete a file entry by ID
  delete(id) {
    const idx = files.findIndex(f => f.id === Number(id));
    if (idx !== -1) { files.splice(idx, 1); return true; }
    return false;
  }
};

module.exports = File;
