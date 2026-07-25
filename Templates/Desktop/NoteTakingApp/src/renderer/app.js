// Escape HTML to prevent XSS attacks
function escapeHtml(text) {
  const div = document.createElement('div');
  div.textContent = text;
  return div.innerHTML;
}

// Renderer logic for the Note Taking App
// Manages folder selection, note CRUD, search, and the editor pane

let currentFolder = 'General';
let currentNoteId = null;
let data = { folders: ['General'], notes: [] };

// Load data from the main process and render the full UI
async function refresh() {
  data = await window.notesAPI.getNotes();
  renderFolders();
  renderNotes();
}

// Render the folder list and highlight the selected folder
function renderFolders() {
  document.getElementById('folderList').innerHTML = data.folders.map(f =>
    `<li class="${f === currentFolder ? 'active' : ''}" onclick="selectFolder('${escapeHtml(f)}')">${escapeHtml(f)}</li>`
  ).join('');
}

// Select a folder and re-render the notes list for it
function selectFolder(folder) {
  currentFolder = folder;
  renderFolders();
  renderNotes();
}

// Create a new folder and refresh the UI
async function addFolder() {
  const input = document.getElementById('folderInput');
  if (input.value.trim()) {
    data = await window.notesAPI.addFolder(input.value.trim());
    input.value = '';
    currentFolder = input.value.trim();
    refresh();
  }
}

// Render notes filtered by selected folder and search query
function renderNotes() {
  const query = document.getElementById('searchInput').value.toLowerCase();
  const filtered = data.notes.filter(n =>
    n.folder === currentFolder && (n.title.toLowerCase().includes(query) || n.content.toLowerCase().includes(query))
  );
  document.getElementById('notesList').innerHTML = filtered.map(n =>
    `<li class="${n.id === currentNoteId ? 'active' : ''}" onclick="selectNote(${n.id})">${escapeHtml(n.title)}</li>`
  ).join('');
}

// Select a note and load its content into the editor pane
function selectNote(id) {
  currentNoteId = id;
  const note = data.notes.find(n => n.id === id);
  if (note) {
    document.getElementById('noteTitle').value = note.title;
    document.getElementById('noteContent').value = note.content;
  }
  renderNotes();
}

// Save the currently edited note, creating or updating as needed
async function saveNote() {
  const title = document.getElementById('noteTitle').value.trim();
  const content = document.getElementById('noteContent').value;
  if (!title) return;
  if (currentNoteId) {
    data = await window.notesAPI.updateNote(currentNoteId, { title, content });
  } else {
    data = await window.notesAPI.addNote({ title, content, folder: currentFolder });
  }
  refresh();
}

// Search input triggers re-render on every keystroke
document.getElementById('searchInput').addEventListener('input', renderNotes);
refresh();
