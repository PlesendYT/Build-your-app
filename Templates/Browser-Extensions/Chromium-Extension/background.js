// Chromium service worker – runs in the background and handles events

// Set default state on first install
chrome.runtime.onInstalled.addListener(() => {
  chrome.storage.local.set({ bookmarks: [], enabled: true });
});

// Handle messages from content scripts and the popup
chrome.runtime.onMessage.addListener((message, sender, sendResponse) => {
  if (message.type === 'SAVE_BOOKMARK') {
    // Persist a bookmark object with the current page URL and title
    chrome.storage.local.get('bookmarks', ({ bookmarks }) => {
      const updated = [...(bookmarks ?? []), { url: message.url, title: message.title, date: Date.now() }];
      chrome.storage.local.set({ bookmarks: updated });
      sendResponse({ success: true, count: updated.length });
    });
    return true;
  }

  if (message.type === 'GET_BOOKMARKS') {
    chrome.storage.local.get('bookmarks', ({ bookmarks }) => {
      sendResponse(bookmarks ?? []);
    });
    return true;
  }
});
