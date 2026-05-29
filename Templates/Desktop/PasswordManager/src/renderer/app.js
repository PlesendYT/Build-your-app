// Renderer process for the Password Manager UI
// Handles unlock flow, credential CRUD operations, and DOM updates

const unlockScreen = document.getElementById('unlockScreen');
const vaultScreen = document.getElementById('vaultScreen');
const unlockError = document.getElementById('unlockError');
const passwordList = document.getElementById('passwordList');
const addForm = document.getElementById('addForm');

// Verify the master password and reveal the vault on success
async function unlock() {
  const pw = document.getElementById('masterPassword').value;
  const valid = await window.passwordAPI.verifyMasterPassword(pw);
  if (valid) {
    unlockScreen.classList.add('hidden');
    vaultScreen.classList.remove('hidden');
    renderPasswords();
  } else {
    unlockError.textContent = 'Incorrect master password';
  }
}

// Fetch and display all stored credentials in the table
async function renderPasswords() {
  const entries = await window.passwordAPI.getPasswords();
  passwordList.innerHTML = entries.map(e => `
    <tr>
      <td>${e.site}</td>
      <td>${e.username}</td>
      <td><button class="danger" onclick="deleteEntry(${e.id})">Delete</button></td>
    </tr>
  `).join('');
}

// Handle add form submission to create a new credential entry
addForm.addEventListener('submit', async (e) => {
  e.preventDefault();
  const form = new FormData(addForm);
  await window.passwordAPI.addPassword({
    site: form.get('site'),
    username: form.get('username'),
    password: form.get('password')
  });
  addForm.reset();
  renderPasswords();
});

// Delete a credential entry by its id and refresh the list
async function deleteEntry(id) {
  await window.passwordAPI.deletePassword(id);
  renderPasswords();
}
