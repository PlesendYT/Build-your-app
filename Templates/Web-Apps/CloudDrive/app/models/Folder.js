// Folder model - in-memory tree structure for cloud drive folders
const folders = [];
let nextId = 1;

const Folder = {
  // Find a folder by ID
  findById(id) {
    return folders.find(f => f.id === Number(id)) || null;
  },

  // Find all subfolders of a given parent
  findByParentId(parentId) {
    const pid = parentId === null ? null : Number(parentId);
    return folders.filter(f => f.parentId === pid);
  },

  // Create a new folder
  create({ name, parentId }) {
    const folder = { id: nextId++, name, parentId: parentId === null ? null : Number(parentId), createdAt: new Date() };
    folders.push(folder);
    return folder;
  },

  // Build breadcrumb path from root to the given folder
  getBreadcrumb(folderId) {
    const crumbs = [];
    let current = folderId ? folders.find(f => f.id === Number(folderId)) : null;
    while (current) {
      crumbs.unshift({ id: current.id, name: current.name });
      current = current.parentId ? folders.find(f => f.id === current.parentId) : null;
    }
    crumbs.unshift({ id: null, name: 'Root' });
    return crumbs;
  },

  // Delete a folder (recursively)
  delete(id) {
    const idsToDelete = [Number(id)];
    // Find all descendants recursively
    const findDescendants = (parentId) => {
      folders.filter(f => f.parentId === parentId).forEach(child => {
        idsToDelete.push(child.id);
        findDescendants(child.id);
      });
    };
    findDescendants(Number(id));
    idsToDelete.forEach(id => {
      const idx = folders.findIndex(f => f.id === id);
      if (idx !== -1) folders.splice(idx, 1);
    });
  }
};

module.exports = Folder;
