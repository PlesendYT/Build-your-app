// Column model - in-memory store for board columns
const columns = [];
let nextId = 1;

const Column = {
  // Find all columns for a given board, sorted by order
  findByBoardId(boardId) {
    return columns.filter(c => c.boardId === Number(boardId)).sort((a, b) => a.order - b.order);
  },

  // Find a single column by ID
  findById(id) {
    return columns.find(c => c.id === Number(id)) || null;
  },

  // Create a new column on a board
  create({ boardId, title, order }) {
    const col = { id: nextId++, boardId: Number(boardId), title, order };
    columns.push(col);
    return col;
  }
};

module.exports = Column;
