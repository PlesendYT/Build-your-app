// Chromium content script – runs on every matched page

// Signal readiness to the service worker
chrome.runtime.sendMessage({ type: 'CONTENT_READY', url: location.href });

// Add page-action controls via a floating button injected into the DOM
const floatBtn = document.createElement('button');
floatBtn.id = 'crx-bookmark-btn';
floatBtn.textContent = '🔖 Bookmark';
Object.assign(floatBtn.style, {
  position: 'fixed', bottom: '16px', right: '16px', zIndex: '99999',
  padding: '8px 14px', borderRadius: '8px', border: 'none',
  background: '#2563eb', color: '#fff', cursor: 'pointer',
  fontSize: '14px', boxShadow: '0 2px 8px rgba(0,0,0,0.2)',
});
document.body.appendChild(floatBtn);

// When the bookmark button is clicked, send the page info to the background
floatBtn.addEventListener('click', () => {
  chrome.runtime.sendMessage({
    type: 'SAVE_BOOKMARK',
    url: location.href,
    title: document.title,
  });
});
