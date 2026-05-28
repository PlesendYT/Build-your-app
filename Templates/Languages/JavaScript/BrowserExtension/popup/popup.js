// Popup Script

const countSpan = document.getElementById('count');
const btnIncrement = document.getElementById('increment');

async function loadCount() {
  const response = await chrome.runtime.sendMessage({ type: 'GET_COUNT' });
  countSpan.textContent = response.count;
}

btnIncrement.addEventListener('click', async () => {
  const response = await chrome.runtime.sendMessage({ type: 'INCREMENT' });
  countSpan.textContent = response.count;
});

loadCount();
