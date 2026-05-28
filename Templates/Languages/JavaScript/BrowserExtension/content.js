// Content Script — läuft auf jeder besuchten Seite

console.log('BrowserExtension content script loaded');

// Aktuelle URL an Background senden
chrome.runtime.sendMessage({
  type: 'PAGE_INFO',
  url: window.location.href,
  title: document.title,
});

// Beispiel: alle Links hervorheben
document.querySelectorAll('a').forEach(link => {
  link.style.outline = '2px solid rgba(74, 144, 217, 0.3)';
});
