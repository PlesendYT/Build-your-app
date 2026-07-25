// Escape HTML to prevent XSS attacks
function escapeHtml(text) {
  const div = document.createElement('div');
  div.textContent = text;
  return div.innerHTML;
}

// Renderer logic for the Video Player
// Handles video playback, drag-drop, playlist management, and controls

const video = document.getElementById('videoEl');
const dropZone = document.getElementById('dropZone');
const dropMsg = document.getElementById('dropMsg');
const controls = document.getElementById('controls');
const playlist = document.getElementById('playlist');
const seek = document.getElementById('seek');
const volume = document.getElementById('volume');
const timeDisplay = document.getElementById('timeDisplay');
const playBtn = document.getElementById('playBtn');
let playlistItems = [];
let currentIndex = 0;

// Format seconds into MM:SS display string
function formatTime(s) {
  const m = Math.floor(s / 60);
  const sec = Math.floor(s % 60);
  return `${m}:${sec.toString().padStart(2, '0')}`;
}

// Load a video source by file path into the video element
function loadVideo(filePath, name) {
  video.src = `file://${filePath}`;
  video.style.display = 'block';
  dropMsg.style.display = 'none';
  controls.classList.add('visible');
  video.load();
  video.play();
  document.title = name;
}

// Add files to the playlist and load the first one
function addToPlaylist(files) {
  files.forEach(f => playlistItems.push({ path: f, name: f.split(/[/\\]/).pop() }));
  renderPlaylist();
  if (playlistItems.length > 0 && !video.src) loadVideo(playlistItems[0].path, playlistItems[0].name);
}

// Render the playlist as a clickable list
function renderPlaylist() {
  playlist.innerHTML = playlistItems.map((item, i) =>
    `<li class="${i === currentIndex ? 'active' : ''}" onclick="loadVideo(playlistItems[${i}].path, playlistItems[${i}].name); currentIndex = ${i};">${escapeHtml(item.name)}</li>`
  ).join('');
}

// Open video files via the system dialog
document.getElementById('openBtn').addEventListener('click', async () => {
  const files = await window.videoAPI.openVideo();
  if (files) addToPlaylist(files);
});

// Handle drag-and-drop of video files onto the drop zone
dropZone.addEventListener('dragover', e => { e.preventDefault(); dropZone.classList.add('dragover'); });
dropZone.addEventListener('dragleave', () => dropZone.classList.remove('dragover'));
dropZone.addEventListener('drop', e => {
  e.preventDefault();
  dropZone.classList.remove('dragover');
  const files = Array.from(e.dataTransfer.files).filter(f => f.name.match(/\.(mp4|webm|mkv|avi|mov)$/i)).map(f => f.path);
  if (files.length) addToPlaylist(files);
});

// Play/pause toggle button
playBtn.addEventListener('click', () => { video.paused ? video.play() : video.pause(); });
video.addEventListener('play', () => { playBtn.textContent = 'Pause'; });
video.addEventListener('pause', () => { playBtn.textContent = 'Play'; });

// Update seek bar and time display during playback
video.addEventListener('timeupdate', () => {
  seek.max = video.duration || 0;
  seek.value = video.currentTime;
  timeDisplay.textContent = `${formatTime(video.currentTime)} / ${formatTime(video.duration)}`;
});

// Seek to a position when the seek bar is changed
seek.addEventListener('input', () => { video.currentTime = seek.value; });
volume.addEventListener('input', () => { video.volume = volume.value; });

// Toggle the playlist panel visibility
document.getElementById('playlistBtn').addEventListener('click', () => playlist.classList.toggle('visible'));
