if ('serviceWorker' in navigator) {
  navigator.serviceWorker.register('/sw.js');
}

let deferredPrompt;
const installBtn = document.getElementById('installBtn');

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

const form = document.getElementById('noteForm');
const input = document.getElementById('noteInput');
const list = document.getElementById('noteList');

async function renderNotes() {
  const notes = await getNotes();
  list.innerHTML = notes.reverse().map(n =>
    `<li>
      <span>${n.text}</span>
      <button class="delete-btn" data-id="${n.id}">✕</button>
    </li>`
  ).join('');

  document.querySelectorAll('.delete-btn').forEach(btn => {
    btn.addEventListener('click', async () => {
      await deleteNote(Number(btn.dataset.id));
      renderNotes();
    });
  });
}

form.addEventListener('submit', async (e) => {
  e.preventDefault();
  const text = input.value.trim();
  if (text) {
    await addNote(text);
    input.value = '';
    renderNotes();
  }
});

renderNotes();
