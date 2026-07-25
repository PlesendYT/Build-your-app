// Escape HTML to prevent XSS attacks
function escapeHtml(text) {
  const div = document.createElement('div');
  div.textContent = text;
  return div.innerHTML;
}

// Renderer logic for the File Explorer
// Handles directory navigation, file listing, and operations

let currentPath = '';
const history = [];
const entriesEl = document.getElementById('entries');
const pathEl = document.getElementById('currentPath');

// Load and display the directory listing for the given path
async function loadDirectory(dirPath) {
  if (!dirPath) dirPath = await window.fileAPI.getHomeDir();
  currentPath = dirPath;
  pathEl.textContent = dirPath;
  const entries = await window.fileAPI.listDirectory(dirPath);
  entriesEl.innerHTML = entries.map(e => `
    <tr>
      <td class="${e.isDirectory ? 'dir' : 'file'}" onclick="${e.isDirectory ? `navigate('${e.path.replace(/'/g, "\\'")}')` : ''}">
        ${e.isDirectory ? '📁 ' : '📄 '}${escapeHtml(e.name)}
      </td>
      <td>${e.isDirectory ? '-' : (e.size / 1024).toFixed(1) + ' KB'}</td>
      <td><button class="danger" onclick="deleteEntry('${e.path.replace(/'/g, "\\'")}')">Delete</button></td>
    </tr>
  `).join('');
}

// Navigate into a directory, pushing the current path to history
function navigate(dirPath) {
  history.push(currentPath);
  loadDirectory(dirPath);
}

// Go back to the previous directory in the navigation history
function goBack() {
  if (history.length) loadDirectory(history.pop());
}

// Prompt for a new folder name and create it in the current directory
async function newFolder() {
  const name = prompt('Folder name:');
  if (name) {
    await window.fileAPI.createDirectory(currentPath, name);
    loadDirectory(currentPath);
  }
}

// Delete the selected file or directory and refresh the listing
async function deleteEntry(targetPath) {
  if (confirm(`Delete ${targetPath}?`)) {
    await window.fileAPI.deleteEntry(targetPath);
    loadDirectory(currentPath);
  }
}

// Start at the home directory on load
loadDirectory();
