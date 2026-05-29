// Firefox content script injected into every matched page

// Send a ready event to the background page
browser.runtime.sendMessage({ type: 'CONTENT_READY', url: location.href });

// Listen for requests from the popup or background script
browser.runtime.onMessage.addListener((message, sender, sendResponse) => {
  if (message.type === 'GET_SELECTION') {
    // Return the user's current text selection on the page
    sendResponse({ selection: window.getSelection()?.toString() ?? '' });
    return true;
  }

  if (message.type === 'HIGHLIGHT_PARAGRAPHS') {
    // Toggle a highlight class on all paragraph elements
    document.querySelectorAll('p').forEach((p) => {
      p.classList.toggle('ff-extension-highlight');
    });
    sendResponse({ toggled: true });
  }
});
