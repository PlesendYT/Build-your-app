// Renderer logic for the App Launcher
// Manages app grid rendering, search filtering, and CRUD operations

let apps = [];
const grid = document.getElementById('appGrid');
const addForm = document.getElementById('addForm');

// Load apps from the main process and render the grid
async function refresh() {
  apps = await window.launcherAPI.getApps();
  renderApps();
}

// Render the app grid filtered by search query
function renderApps() {
  const query = document.getElementById('searchBar').value.toLowerCase();
  const filtered = apps.filter(a => a.name.toLowerCase().includes(query));
  grid.innerHTML = filtered.map(a => `
    <div class="app-card">
      <h3>${a.name}</h3>
      <p>${a.path}</p>
      <button class="remove-btn" onclick="removeApp(${a.id})">Remove</button>
    </div>
  `).join('');
}

// Handle add form submission to register a new app
addForm.addEventListener('submit', async (e) => {
  e.preventDefault();
  const form = new FormData(addForm);
  apps = await window.launcherAPI.addApp({
    name: form.get('name'),
    path: form.get('path')
  });
  addForm.reset();
  renderApps();
});

// Remove an app by its id and refresh the grid
async function removeApp(id) {
  apps = await window.launcherAPI.removeApp(id);
  renderApps();
}

// Launch an app when its card is clicked
grid.addEventListener('click', (e) => {
  const card = e.target.closest('.app-card');
  if (card && !e.target.classList.contains('remove-btn')) {
    const name = card.querySelector('h3').textContent;
    const app = apps.find(a => a.name === name);
    if (app) window.launcherAPI.launchApp(app.path);
  }
});

refresh();
