// Firefox popup script – communicates with content and background scripts

// Helper to send a message to the active tab's content script
async function sendToActiveTab(message) {
  const tabs = await browser.tabs.query({ active: true, currentWindow: true });
  if (!tabs[0]?.id) return null;
  return browser.tabs.sendMessage(tabs[0].id, message);
}

// Fetch the user's current text selection from the page
document.getElementById('getSelectionBtn')?.addEventListener('click', async () => {
  const response = await sendToActiveTab({ type: 'GET_SELECTION' });
  const display = document.getElementById('selectionDisplay');
  if (display) {
    display.textContent = response?.selection || 'Nothing selected.';
  }
});

// Toggle the extension enabled/disabled state via the background script
document.getElementById('toggleBtn')?.addEventListener('click', () => {
  browser.runtime.sendMessage({ type: 'TOGGLE_ENABLED' });
});

// Ask the content script to highlight or unhighlight paragraphs
document.getElementById('highlightBtn')?.addEventListener('click', async () => {
  await sendToActiveTab({ type: 'HIGHLIGHT_PARAGRAPHS' });
});
