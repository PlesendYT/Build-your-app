// Popup script — runs when the extension icon is clicked
// Displays a counter and lets the user increment it via the background worker

const countSpan = document.getElementById('count');
const btnIncrement = document.getElementById('increment');

/** Fetches the current counter value from the background worker */
async function loadCount() {
  const response = await chrome.runtime.sendMessage({ type: 'GET_COUNT' });
  countSpan.textContent = response.count;
}

// Increment the counter on button click
btnIncrement.addEventListener('click', async () => {
  const response = await chrome.runtime.sendMessage({ type: 'INCREMENT' });
  countSpan.textContent = response.count;
});

// Load initial count when popup opens
loadCount();
