// Board model - in-memory store for Kanban boards
const boards = [];
let nextId = 1;

const Board = {
  // Get all boards
  findAll() {
    return boards;
  },

  // Find a board by ID
  findById(id) {
    return boards.find(b => b.id === Number(id)) || null;
  },

  // Create a new board
  create(data = {}) {
    const board = { id: nextId++, title: data.title || 'New Board', createdAt: new Date() };
    boards.push(board);
    return board;
  }
};

module.exports = Board;
