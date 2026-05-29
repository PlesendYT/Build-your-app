// Popup script – runs when the user clicks the extension toolbar icon

interface PageInfo {
  title: string;
  url: string;
}

// Query the active tab and request page information from its content script
async function getCurrentPageInfo(): Promise<PageInfo | null> {
  const [tab] = await chrome.tabs.query({ active: true, currentWindow: true });
  if (!tab?.id) return null;

  try {
    const response = await chrome.tabs.sendMessage(tab.id, { type: 'GET_PAGE_INFO' });
    return response as PageInfo;
  } catch {
    // Content script may not be injected on restricted pages
    return null;
  }
}

// Render the retrieved page information into the popup UI
async function renderPageInfo(): Promise<void> {
  const info = await getCurrentPageInfo();
  const container = document.getElementById('pageInfo');
  if (!container) return;

  if (info) {
    container.innerHTML = `<strong>Title:</strong> ${escapeHtml(info.title)}<br>
                           <strong>URL:</strong> ${escapeHtml(info.url)}`;
  } else {
    container.textContent = 'Page info unavailable (restricted page)';
  }
}

// Basic HTML escaping to prevent XSS when inserting page data
function escapeHtml(text: string): string {
  const div = document.createElement('div');
  div.textContent = text;
  return div.innerHTML;
}

// Initialize the popup when the DOM is ready
document.addEventListener('DOMContentLoaded', renderPageInfo);
