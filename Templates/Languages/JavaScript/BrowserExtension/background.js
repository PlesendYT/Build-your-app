// Background service worker — persists across browser sessions
// Handles extension lifecycle and message-based counter operations

// Initialize storage on first install
chrome.runtime.onInstalled.addListener(({ reason }) => {
  if (reason === 'install') {
    console.log('Extension installed');
    chrome.storage.local.set({ count: 0 });
  }
});

// Listen for messages from popup and content scripts
chrome.runtime.onMessage.addListener((message, sender, sendResponse) => {
  if (message.type === 'GET_COUNT') {
    chrome.storage.local.get('count', (data) => {
      sendResponse({ count: data.count ?? 0 });
    });
    return true; // Keep channel open for async response
  }

  if (message.type === 'INCREMENT') {
    chrome.storage.local.get('count', (data) => {
      const newCount = (data.count ?? 0) + 1;
      chrome.storage.local.set({ count: newCount }, () => {
        sendResponse({ count: newCount });
      });
    });
    return true; // Keep channel open for async response
  }
});
