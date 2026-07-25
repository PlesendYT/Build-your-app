// Escape HTML to prevent XSS attacks
function escapeHtml(text) {
  const div = document.createElement('div');
  div.textContent = text;
  return div.innerHTML;
}

// Cloud Drive client - folder navigation, upload, and file sharing
let currentFolderId = null;
const itemList = document.getElementById('item-list');
const breadcrumb = document.getElementById('breadcrumb');
const fileInput = document.getElementById('file-input');

// Navigate to a folder and load its contents
async function navigateTo(folderId) {
  currentFolderId = folderId;
  const id = folderId === null ? 'root' : folderId;
  const res = await fetch(`/api/drive/folder/${id}`);
  const data = await res.json();
  renderBreadcrumb(data.breadcrumb);
  renderItems(data.folders, data.files);
}

// Render breadcrumb navigation
function renderBreadcrumb(crumbs) {
  breadcrumb.innerHTML = crumbs.map((c, i) => {
    const isLast = i === crumbs.length - 1;
    return isLast
      ? `<span>${escapeHtml(c.name)}</span>`
      : `<a href="#" data-id="${c.id}">${escapeHtml(c.name)}</a> <span>/</span> `;
  }).join('');
  breadcrumb.querySelectorAll('a').forEach(a => {
    a.addEventListener('click', (e) => {
      e.preventDefault();
      navigateTo(a.dataset.id === 'null' ? null : Number(a.dataset.id));
    });
  });
}

// Render folders and files in the current view
function renderItems(folders, files) {
  itemList.innerHTML = '';
  // Render folders first
  folders.forEach(f => {
    const div = document.createElement('div');
    div.className = 'folder-item';
    div.innerHTML = `<span><span class="icon">📁</span>${escapeHtml(f.name)}</span><span class="meta">Folder</span>`;
    div.addEventListener('click', () => navigateTo(f.id));
    itemList.appendChild(div);
  });
  // Render files
  files.forEach(f => {
    const div = document.createElement('div');
    div.className = 'file-item';
    div.innerHTML = `
      <span><span class="icon">📄</span>${escapeHtml(f.originalName)} <span class="meta">(${(f.size / 1024).toFixed(1)} KB)</span></span>
      <div class="actions-right">
        <a href="/api/drive/download/${f.shareCode}">Download</a>
        <a href="#" class="share-link" data-id="${f.id}">Share</a>
        <a href="#" class="delete-link" data-id="${f.id}">Delete</a>
      </div>`;
    itemList.appendChild(div);
  });
  if (folders.length === 0 && files.length === 0) {
    itemList.innerHTML = '<div class="folder-item">This folder is empty</div>';
  }
  // Attach share and delete handlers
  document.querySelectorAll('.share-link').forEach(a => {
    a.addEventListener('click', async (e) => {
      e.preventDefault();
      const res = await fetch(`/api/drive/file/${a.dataset.id}/share`);
      const data = await res.json();
      const shareUrl = `${window.location.origin}${data.downloadUrl}`;
      await navigator.clipboard.writeText(shareUrl);
      alert(`Share link copied: ${shareUrl}`);
    });
  });
  document.querySelectorAll('.delete-link').forEach(a => {
    a.addEventListener('click', async (e) => {
      e.preventDefault();
      await fetch(`/api/drive/file/${a.dataset.id}`, { method: 'DELETE' });
      navigateTo(currentFolderId);
    });
  });
}

// Create a new folder
document.getElementById('new-folder-btn').addEventListener('click', async () => {
  const name = prompt('Folder name:');
  if (name) {
    await fetch('/api/drive/folder', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ name, parentId: currentFolderId })
    });
    navigateTo(currentFolderId);
  }
});

// Upload a file
document.getElementById('upload-btn').addEventListener('click', () => fileInput.click());
fileInput.addEventListener('change', async () => {
  if (fileInput.files.length === 0) return;
  const formData = new FormData();
  formData.append('file', fileInput.files[0]);
  formData.append('folderId', currentFolderId || '');
  await fetch('/api/drive/upload', { method: 'POST', body: formData });
  fileInput.value = '';
  navigateTo(currentFolderId);
});

// Start at root
navigateTo(null);
