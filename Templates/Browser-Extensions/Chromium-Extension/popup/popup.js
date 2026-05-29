// Chromium popup script – renders bookmarks and controls

// Fetch all bookmarks from the background script and render them
async function renderBookmarks() {
  const bookmarks = await chrome.runtime.sendMessage({ type: 'GET_BOOKMARKS' });
  const list = document.getElementById('bookmarkList');
  if (!list) return;

  if (!bookmarks?.length) {
    list.innerHTML = '<li>No bookmarks yet.</li>';
    return;
  }

  list.innerHTML = bookmarks
    .map((b) => `<li><a href="${escapeHtml(b.url)}" target="_blank">${escapeHtml(b.title)}</a></li>`)
    .join('');
}

// Basic HTML entity escaping for safe innerHTML insertion
function escapeHtml(text) {
  const el = document.createElement('span');
  el.textContent = text;
  return el.innerHTML;
}

// Inject a CSS rule into the active tab to highlight all links
async function highlightLinks() {
  const [tab] = await chrome.tabs.query({ active: true, currentWindow: true });
  if (!tab?.id) return;

  await chrome.scripting.insertCSS({
    target: { tabId: tab.id },
    css: 'a { background: yellow !important; outline: 2px solid orange !important; }',
  });
}

// Attach event listeners after DOM is ready
document.addEventListener('DOMContentLoaded', () => {
  renderBookmarks();
  document.getElementById('refreshBtn')?.addEventListener('click', renderBookmarks);
  document.getElementById('highlightLinksBtn')?.addEventListener('click', highlightLinks);
});
