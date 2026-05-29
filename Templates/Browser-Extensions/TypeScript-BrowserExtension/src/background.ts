// Listens for extension installation and tab updates
chrome.runtime.onInstalled.addListener(({ reason }) => {
  // Log the installation reason for debugging purposes
  console.log(`Extension installed: ${reason}`);
});

// Respond to messages from content scripts or popup
chrome.runtime.onMessage.addListener((message, sender, sendResponse) => {
  // Handle simple ping/pong for connection verification
  if (message.type === 'PING') {
    sendResponse({ type: 'PONG' });
    return true;
  }

  // Forward extension state to the requester
  if (message.type === 'GET_STATE') {
    chrome.storage.local.get('state', ({ state }) => {
      sendResponse({ type: 'STATE', data: state });
    });
    return true;
  }
});
