// Card model - in-memory store for Kanban cards with position tracking
const cards = [];
let nextId = 1;

const Card = {
  // Find all cards in a column, sorted by order
  findByColumnId(columnId) {
    return cards.filter(c => c.columnId === Number(columnId)).sort((a, b) => a.order - b.order);
  },

  // Create a new card in a column
  create({ columnId, title, order }) {
    const card = { id: nextId++, columnId: Number(columnId), title, description: '', order };
    cards.push(card);
    return card;
  },

  // Update a card's fields
  update(id, data) {
    const card = cards.find(c => c.id === Number(id));
    if (!card) return null;
    if (data.title !== undefined) card.title = data.title;
    if (data.description !== undefined) card.description = data.description;
    return card;
  },

  // Update card position (for drag-drop reordering)
  updatePosition(id, columnId, order) {
    const card = cards.find(c => c.id === Number(id));
    if (!card) return null;
    card.columnId = Number(columnId);
    card.order = order;
    return card;
  },

  // Delete a card by ID
  delete(id) {
    const idx = cards.findIndex(c => c.id === Number(id));
    if (idx !== -1) cards.splice(idx, 1);
  }
};

module.exports = Card;
