// ChatApp server - Socket.io chat with rooms and message history
const express = require('express');
const http = require('http');
const { Server } = require('socket.io');

const app = express();
const server = http.createServer(app);
const io = new Server(server);

// Serve static files from the public directory
app.use(express.static('public'));

// In-memory message history per room
const messageHistory = {};

// Handle WebSocket connections
io.on('connection', (socket) => {
  console.log(`User connected: ${socket.id}`);

  // Join a specific chat room
  socket.on('joinRoom', (room) => {
    socket.join(room);
    // Send existing history to the new user
    socket.emit('history', messageHistory[room] || []);
  });

  // Handle incoming chat messages
  socket.on('chatMessage', ({ room, username, message }) => {
    const msg = { username, message, timestamp: Date.now() };
    // Store message in room history (keep last 100)
    if (!messageHistory[room]) messageHistory[room] = [];
    messageHistory[room].push(msg);
    if (messageHistory[room].length > 100) messageHistory[room].shift();
    // Broadcast message to everyone in the room
    io.to(room).emit('message', msg);
  });

  socket.on('disconnect', () => {
    console.log(`User disconnected: ${socket.id}`);
  });
});

// Start the server on port 3000
const PORT = process.env.PORT || 3000;
server.listen(PORT, () => console.log(`Chat server running on port ${PORT}`));
