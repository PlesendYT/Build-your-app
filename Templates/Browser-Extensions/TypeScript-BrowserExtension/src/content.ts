// Runs in the context of every page matching the manifest pattern

// Notify the background script that this content script has loaded
chrome.runtime.sendMessage({ type: 'CONTENT_LOADED', url: location.href });

// Apply a subtle visual marker on the page to confirm injection
const marker = document.createElement('div');
marker.id = 'ts-extension-marker';
marker.style.display = 'none';
document.body.appendChild(marker);

// Listen for messages from the background script or popup
chrome.runtime.onMessage.addListener((message, _sender, sendResponse) => {
  // Return the current page title when requested
  if (message.type === 'GET_PAGE_INFO') {
    sendResponse({ title: document.title, url: location.href });
    return true;
  }
});
