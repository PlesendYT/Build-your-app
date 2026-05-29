// Content script injected into every page matching the manifest URL patterns

// Send a ready signal to the background service worker
chrome.runtime.sendMessage({ type: 'CONTENT_READY', url: location.href });

// Highlight the page body to give visual feedback that the script is active
document.body.style.outline = '2px solid #2563eb';
document.body.style.outlineOffset = '-2px';

// Expose a simple API for the popup to retrieve page metadata
chrome.runtime.onMessage.addListener((message, _sender, sendResponse) => {
  if (message.type === 'FETCH_META') {
    const meta = {
      title: document.title,
      description: getMetaContent('description'),
      ogImage: getMetaContent('og:image'),
    };
    sendResponse(meta);
    return true;
  }
});

// Helper to extract the content attribute from a meta tag by name or property
function getMetaContent(name: string): string | null {
  const el = document.querySelector(`meta[name="${name}"], meta[property="${name}"]`);
  return el?.getAttribute('content') ?? null;
}
