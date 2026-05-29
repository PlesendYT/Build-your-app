// File hosting client - handle file uploads and list files
const dropZone = document.getElementById('drop-zone');
const fileInput = document.getElementById('file-input');
const uploadBtn = document.getElementById('upload-btn');
const fileList = document.getElementById('file-list');

// Open file picker when upload button is clicked
uploadBtn.addEventListener('click', () => fileInput.click());

// Upload a file to the server
async function uploadFile(file) {
  const formData = new FormData();
  formData.append('file', file);
  try {
    const res = await fetch('/api/files/upload', { method: 'POST', body: formData });
    if (res.ok) {
      loadFiles();
    }
  } catch (err) {
    console.error('Upload failed:', err);
  }
}

// Handle file selection from the picker
fileInput.addEventListener('change', () => {
  if (fileInput.files.length > 0) {
    uploadFile(fileInput.files[0]);
    fileInput.value = '';
  }
});

// Drag-drop handlers for the upload zone
dropZone.addEventListener('dragover', (e) => { e.preventDefault(); dropZone.classList.add('dragover'); });
dropZone.addEventListener('dragleave', () => dropZone.classList.remove('dragover'));
dropZone.addEventListener('drop', (e) => {
  e.preventDefault();
  dropZone.classList.remove('dragover');
  if (e.dataTransfer.files.length > 0) uploadFile(e.dataTransfer.files[0]);
});

// Load and display the list of uploaded files
async function loadFiles() {
  const res = await fetch('/api/files');
  const files = await res.json();
  fileList.innerHTML = files.map(f => `
    <div class="file-item">
      <div class="info">
        <div class="name">${f.originalName}</div>
        <div class="meta">${(f.size / 1024).toFixed(1)} KB | ${f.downloads} downloads</div>
      </div>
      <div class="actions">
        <a href="/api/files/download/${f.shareCode}">Download</a>
      </div>
    </div>
  `).join('');
  if (files.length === 0) fileList.innerHTML = '<div class="file-item">No files uploaded yet.</div>';
}

// Initial load
loadFiles();
