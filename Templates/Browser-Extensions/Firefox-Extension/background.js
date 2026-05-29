// Firefox extension background script (non-persistent event page)

// Log extension installation and set default settings
browser.runtime.onInstalled.addListener(({ reason }) => {
  console.log(`Extension installed: ${reason}`);
  browser.storage.local.set({ theme: 'light', enabled: true });
});

// Respond to messages from content scripts or the popup
browser.runtime.onMessage.addListener((message, sender, sendResponse) => {
  if (message.type === 'GET_SETTINGS') {
    browser.storage.local.get(['theme', 'enabled'], (settings) => {
      sendResponse(settings);
    });
    return true;
  }

  if (message.type === 'TOGGLE_ENABLED') {
    browser.storage.local.get('enabled', ({ enabled }) => {
      browser.storage.local.set({ enabled: !enabled });
    });
  }
});
