// KanbanBoard client - manages board state with drag-drop and API calls
const API = '/api/boards';
let currentBoard = null;

// Fetch or create a board on load
async function init() {
  let res = await fetch(API);
  let boards = await res.json();
  if (boards.length === 0) {
    res = await fetch(API, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ title: 'My Board' }) });
    const board = await res.json();
    currentBoard = board;
    // Create default column titles via API
  } else {
    currentBoard = boards[0];
  }
  loadBoard();
}

// Load board data and render columns and cards
async function loadBoard() {
  const res = await fetch(`${API}/${currentBoard.id}`);
  const data = await res.json();
  renderBoard(data);
}

// Escape HTML to prevent XSS attacks
function escapeHtml(text) {
  const div = document.createElement('div');
  div.textContent = text;
  return div.innerHTML;
}

// Render all columns and their cards
function renderBoard(board) {
  const container = document.getElementById('board-container');
  container.innerHTML = '';
  board.columns.forEach(col => {
    const colEl = document.createElement('div');
    colEl.className = 'column';
    colEl.dataset.columnId = col.id;
    colEl.innerHTML = `<div class="column-header"><span>${escapeHtml(col.title)}</span><span></span></div>`;
    // Render each card in the column
    col.cards.forEach(card => {
      colEl.appendChild(createCardElement(card));
    });
    // Add card button
    const addBtn = document.createElement('button');
    addBtn.className = 'add-btn';
    addBtn.textContent = '+ Add Card';
    addBtn.addEventListener('click', () => addCard(col.id));
    colEl.appendChild(addBtn);
    container.appendChild(colEl);
  });
}

// Create a draggable card element
function createCardElement(card) {
  const div = document.createElement('div');
  div.className = 'card';
  div.draggable = true;
  div.dataset.cardId = card.id;
  div.innerHTML = `<span>${escapeHtml(card.title)}</span><span class="delete-btn" data-id="${card.id}">✕</span>`;
  // Drag event handlers for reordering
  div.addEventListener('dragstart', () => div.classList.add('dragging'));
  div.addEventListener('dragend', () => div.classList.remove('dragging'));
  div.addEventListener('dragover', (e) => e.preventDefault());
  // Delete card handler
  div.querySelector('.delete-btn').addEventListener('click', async (e) => {
    e.stopPropagation();
    await fetch(`/api/boards/cards/${card.id}`, { method: 'DELETE' });
    loadBoard();
  });
  // Drop handler to update card position
  div.addEventListener('drop', async (e) => {
    e.preventDefault();
    const dragging = document.querySelector('.dragging');
    if (dragging) {
      const cardId = dragging.dataset.cardId;
      const columnId = div.closest('.column').dataset.columnId;
      const order = Array.from(div.parentNode.children).indexOf(div);
      await fetch(`/api/boards/cards/${cardId}/position`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ columnId: Number(columnId), order })
      });
      loadBoard();
    }
  });
  return div;
}

// Add a new card to a column
async function addCard(columnId) {
  const title = prompt('Card title:');
  if (title) {
    await fetch(`/api/boards/columns/${columnId}/cards`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ title })
    });
    loadBoard();
  }
}

// Add column button handler
document.getElementById('add-column-btn').addEventListener('click', async () => {
  const title = prompt('Column title:');
  if (title && currentBoard) {
    await fetch(`${API}/${currentBoard.id}/columns`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ title })
    });
    loadBoard();
  }
});

init();
