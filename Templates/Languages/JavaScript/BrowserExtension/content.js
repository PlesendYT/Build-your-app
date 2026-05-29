// Content script — injected into every page the user visits
// Sends page info to the background script and highlights all links

console.log('BrowserExtension content script loaded');

// Send current page URL and title to the background service worker
chrome.runtime.sendMessage({
  type: 'PAGE_INFO',
  url: window.location.href,
  title: document.title,
});

// Highlight all anchor elements with a subtle blue outline
document.querySelectorAll('a').forEach(link => {
  link.style.outline = '2px solid rgba(74, 144, 217, 0.3)';
});
