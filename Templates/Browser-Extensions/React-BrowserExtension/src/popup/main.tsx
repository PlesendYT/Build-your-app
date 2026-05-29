import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';

// Render the root React component inside the popup"s mount element
const root = document.getElementById('root');
if (root) {
  ReactDOM.createRoot(root).render(
    <React.StrictMode>
      <App />
    </React.StrictMode>,
  );
}
