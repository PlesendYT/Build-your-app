// Service worker for the React extension – handles lifecycle and messaging

// Initialise default storage values when the extension is installed
chrome.runtime.onInstalled.addListener(() => {
  chrome.storage.local.set({ count: 0, enabled: true });
});

// Listen for messages forwarded from content scripts or the popup
chrome.runtime.onMessage.addListener((message, _sender, sendResponse) => {
  if (message.type === 'INCREMENT') {
    // Atomically increment a counter stored in local storage
    chrome.storage.local.get('count', ({ count }) => {
      const next = (count ?? 0) + 1;
      chrome.storage.local.set({ count: next });
      sendResponse({ count: next });
    });
    return true;
  }
});
