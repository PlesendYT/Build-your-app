// Register the service worker if the browser supports it
if ('serviceWorker' in navigator) {
  navigator.serviceWorker.register('/sw.js');
}

// Deferred prompt and install button for PWA install banner
let deferredPrompt;
const installBtn = document.getElementById('installBtn');

// Listen for the beforeinstallprompt event to show the install button
window.addEventListener('beforeinstallprompt', (e) => {
  e.preventDefault();
  deferredPrompt = e;
  installBtn.classList.remove('hidden');
  installBtn.addEventListener('click', async () => {
    installBtn.classList.add('hidden');
    deferredPrompt.prompt();
    const result = await deferredPrompt.userChoice;
    deferredPrompt = null;
  });
});

// DOM references for the note form, input, and list
const form = document.getElementById('noteForm');
const input = document.getElementById('noteInput');
const list = document.getElementById('noteList');

/** Fetch all notes from IndexedDB and render them as a list with delete buttons */
async function renderNotes() {
  const notes = await getNotes();
  list.innerHTML = notes.reverse().map(n =>
    `<li>
      <span>${n.text}</span>
      <button class="delete-btn" data-id="${n.id}">✕</button>
    </li>`
  ).join('');

  // Attach click handlers to each delete button
  document.querySelectorAll('.delete-btn').forEach(btn => {
    btn.addEventListener('click', async () => {
      await deleteNote(Number(btn.dataset.id));
      renderNotes();
    });
  });
}

// Handle form submission to add a new note
form.addEventListener('submit', async (e) => {
  e.preventDefault();
  const text = input.value.trim();
  if (text) {
    await addNote(text);
    input.value = '';
    renderNotes();
  }
});

// Initial render of notes on page load
renderNotes();
