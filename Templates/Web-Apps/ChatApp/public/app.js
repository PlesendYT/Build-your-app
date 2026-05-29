// ChatApp client - connect to Socket.io server and handle UI events
const socket = io();

// DOM element references
const messagesEl = document.getElementById('messages');
const messageInput = document.getElementById('message-input');
const usernameInput = document.getElementById('username-input');
const roomInput = document.getElementById('room-input');
const roomLabel = document.getElementById('room-label');
const sendBtn = document.getElementById('send-btn');

let currentRoom = 'Lobby';

// Join a room and update the UI label
function joinRoom(room) {
  currentRoom = room;
  roomLabel.textContent = room;
  messagesEl.innerHTML = '';
  socket.emit('joinRoom', room);
}

// Receive message history when joining a room
socket.on('history', (history) => {
  history.forEach(msg => displayMessage(msg));
});

// Receive a new chat message
socket.on('message', (msg) => {
  displayMessage(msg);
});

// Add a message to the UI
function displayMessage(msg) {
  const div = document.createElement('div');
  div.className = 'message';
  div.innerHTML = `<strong>${msg.username}</strong> <span class="time">${new Date(msg.timestamp).toLocaleTimeString()}</span><p>${msg.message}</p>`;
  messagesEl.appendChild(div);
  messagesEl.scrollTop = messagesEl.scrollHeight;
}

// Join a room when the room input changes
roomInput.addEventListener('change', () => {
  joinRoom(roomInput.value || 'Lobby');
});

// Send message on button click or Enter key
sendBtn.addEventListener('click', sendMessage);
messageInput.addEventListener('keypress', (e) => {
  if (e.key === 'Enter') sendMessage();
});

function sendMessage() {
  const msg = messageInput.value.trim();
  if (msg) {
    socket.emit('chatMessage', {
      room: currentRoom,
      username: usernameInput.value || 'Anonymous',
      message: msg
    });
    messageInput.value = '';
  }
}

// Join the default room on load
joinRoom('Lobby');
