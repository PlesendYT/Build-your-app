// Board API routes - CRUD for boards, columns, and cards with drag-drop ordering
const express = require('express');
const router = express.Router();
const Board = require('../models/Board');
const Column = require('../models/Column');
const Card = require('../models/Card');

// Get all boards
router.get('/', (req, res) => {
  res.json(Board.findAll());
});

// Create a new board
router.post('/', (req, res) => {
  const board = Board.create(req.body);
  // Create default columns for the new board
  Column.create({ boardId: board.id, title: 'To Do', order: 0 });
  Column.create({ boardId: board.id, title: 'In Progress', order: 1 });
  Column.create({ boardId: board.id, title: 'Done', order: 2 });
  res.status(201).json(board);
});

// Get a single board with its columns and cards
router.get('/:id', (req, res) => {
  const board = Board.findById(req.params.id);
  if (!board) return res.status(404).json({ error: 'Board not found' });
  const columns = Column.findByBoardId(board.id);
  const cards = columns.map(col => ({
    ...col,
    cards: Card.findByColumnId(col.id)
  }));
  res.json({ ...board, columns: cards });
});

// Create a new column on a board
router.post('/:id/columns', (req, res) => {
  const columns = Column.findByBoardId(req.params.id);
  const col = Column.create({ boardId: Number(req.params.id), title: req.body.title, order: columns.length });
  res.status(201).json(col);
});

// Update card position (drag-drop reordering)
router.put('/cards/:id/position', (req, res) => {
  const { columnId, order } = req.body;
  const card = Card.updatePosition(req.params.id, columnId, order);
  if (!card) return res.status(404).json({ error: 'Card not found' });
  res.json(card);
});

// Create a new card in a column
router.post('/columns/:id/cards', (req, res) => {
  const cards = Card.findByColumnId(req.params.id);
  const card = Card.create({ columnId: Number(req.params.id), title: req.body.title, order: cards.length });
  res.status(201).json(card);
});

// Update a card's details
router.put('/cards/:id', (req, res) => {
  const card = Card.update(req.params.id, req.body);
  if (!card) return res.status(404).json({ error: 'Card not found' });
  res.json(card);
});

// Delete a card
router.delete('/cards/:id', (req, res) => {
  Card.delete(req.params.id);
  res.status(204).end();
});

module.exports = router;
