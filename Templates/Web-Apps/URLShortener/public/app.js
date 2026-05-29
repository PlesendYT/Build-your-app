// URL Shortener client - send URLs to API and display shortened results
const urlInput = document.getElementById('url-input');
const shortenBtn = document.getElementById('shorten-btn');
const resultDiv = document.getElementById('result');
const shortLink = document.getElementById('short-link');
const statsEl = document.getElementById('stats');

// Handle the shorten button click
shortenBtn.addEventListener('click', async () => {
  const url = urlInput.value.trim();
  if (!url) return alert('Please enter a URL');

  try {
    // Send URL to the server for shortening
    const res = await fetch('/api/shorten', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ url })
    });

    if (!res.ok) throw new Error('Failed to shorten');

    const data = await res.json();
    // Display the shortened URL and fetch stats
    shortLink.href = data.shortUrl;
    shortLink.textContent = data.shortUrl;
    resultDiv.style.display = 'block';
    loadStats(data.code);
  } catch (err) {
    alert('Error: ' + err.message);
  }
});

// Load and display click stats for a short code
async function loadStats(code) {
  const res = await fetch(`/api/stats/${code}`);
  const data = await res.json();
  statsEl.textContent = `Clicks: ${data.clicks} | Created: ${new Date(data.createdAt).toLocaleDateString()}`;
}

// Also allow pressing Enter to submit
urlInput.addEventListener('keypress', (e) => {
  if (e.key === 'Enter') shortenBtn.click();
});
