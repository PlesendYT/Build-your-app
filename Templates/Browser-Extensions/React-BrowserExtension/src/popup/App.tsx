import { useEffect, useState } from 'react';

interface PageMeta {
  title: string;
  description: string | null;
  ogImage: string | null;
}

// Main popup component that displays metadata from the active tab
export default function App() {
  const [meta, setMeta] = useState<PageMeta | null>(null);
  const [count, setCount] = useState(0);

  // Fetch page metadata from the content script on mount
  useEffect(() => {
    (async () => {
      const [tab] = await chrome.tabs.query({ active: true, currentWindow: true });
      if (!tab?.id) return;
      const response = await chrome.tabs.sendMessage(tab.id, { type: 'FETCH_META' });
      setMeta(response as PageMeta);
    })();
  }, []);

  // Send an INCREMENT message to the background script and update the local count
  const handleIncrement = async () => {
    const response = await chrome.runtime.sendMessage({ type: 'INCREMENT' });
    setCount(response.count);
  };

  // Style object for the popup container
  const containerStyle: React.CSSProperties = {
    width: 300,
    padding: 16,
    fontFamily: 'system-ui, sans-serif',
    fontSize: 14,
  };

  return (
    <div style={containerStyle}>
      <h1 style={{ fontSize: 18, margin: '0 0 8px', color: '#2563eb' }}>React Extension</h1>
      {meta ? (
        <div>
          <p><strong>Title:</strong> {meta.title}</p>
          <p><strong>Description:</strong> {meta.description ?? 'N/A'}</p>
        </div>
      ) : (
        <p>Loading page info...</p>
      )}
      <hr style={{ margin: '12px 0' }} />
      <p>Counter: {count}</p>
      <button onClick={handleIncrement}>Increment</button>
    </div>
  );
}
