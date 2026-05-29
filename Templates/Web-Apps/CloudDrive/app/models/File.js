// File model - in-memory store for cloud drive file metadata
const files = [];
let nextId = 1;

const File = {
  // Find all files in a folder
  findByFolderId(folderId) {
    const fid = folderId === null ? null : Number(folderId);
    return files.filter(f => f.folderId === fid);
  },

  // Find a file by ID
  findById(id) {
    return files.find(f => f.id === Number(id)) || null;
  },

  // Find a file by share code
  findByShareCode(code) {
    return files.find(f => f.shareCode === code) || null;
  },

  // Create a new file entry
  create({ originalName, storedName, size, mimeType, folderId, shareCode }) {
    const file = { id: nextId++, originalName, storedName, size, mimeType, folderId: folderId === null ? null : Number(folderId), shareCode, downloads: 0, createdAt: new Date() };
    files.push(file);
    return file;
  },

  // Increment download counter
  incrementDownloads(id) {
    const file = files.find(f => f.id === Number(id));
    if (file) file.downloads++;
  },

  // Delete a file by ID
  delete(id) {
    const idx = files.findIndex(f => f.id === Number(id));
    if (idx !== -1) files.splice(idx, 1);
  },

  // Delete all files in a folder
  deleteByFolderId(folderId) {
    const fid = Number(folderId);
    for (let i = files.length - 1; i >= 0; i--) {
      if (files[i].folderId === fid) files.splice(i, 1);
    }
  }
};

module.exports = File;
