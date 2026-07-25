// Escape HTML to prevent XSS attacks
function escapeHtml(text) {
  const div = document.createElement('div');
  div.textContent = text;
  return div.innerHTML;
}

// Renderer logic for the Music Player
// Manages audio playback, library browsing, search, and playlist queue

const audio = document.getElementById('audioEl');
const library = document.getElementById('library');
const playBtn = document.getElementById('playBtn');
const nowPlaying = document.getElementById('nowPlaying');
const timeDisplay = document.getElementById('timeDisplay');
const seek = document.getElementById('seek');
const volume = document.getElementById('volume');
let tracks = [];
let currentIndex = -1;

// Format seconds into MM:SS display string
function formatTime(s) {
  const m = Math.floor(s / 60);
  const sec = Math.floor(s % 60);
  return `${m}:${sec.toString().padStart(2, '0')}`;
}

// Open directory dialog and scan for audio files to populate the library
async function scanLibrary() {
  tracks = await window.musicAPI.scanLibrary();
  renderLibrary();
  if (tracks.length > 0) loadTrack(0);
}

// Render the library filtered by search query
function renderLibrary() {
  const query = document.getElementById('searchInput').value.toLowerCase();
  const filtered = tracks.filter(t => t.name.toLowerCase().includes(query));
  library.innerHTML = filtered.map((t, i) =>
    `<li class="${t === tracks[currentIndex] ? 'playing' : ''}" onclick="loadTrack(${tracks.indexOf(t)})">${escapeHtml(t.name)}</li>`
  ).join('');
}

// Load a track by index and start playback
function loadTrack(index) {
  if (index < 0 || index >= tracks.length) return;
  currentIndex = index;
  const track = tracks[currentIndex];
  audio.src = `file://${track.path}`;
  audio.load();
  audio.play();
  nowPlaying.textContent = track.name;
  renderLibrary();
}

// Play/pause toggle
playBtn.addEventListener('click', () => { audio.paused ? audio.play() : audio.pause(); });
audio.addEventListener('play', () => { playBtn.textContent = 'Pause'; });
audio.addEventListener('pause', () => { playBtn.textContent = 'Play'; });

// Update seek bar and time display during playback
audio.addEventListener('timeupdate', () => {
  seek.max = audio.duration || 0;
  seek.value = audio.currentTime;
  timeDisplay.textContent = `${formatTime(audio.currentTime)} / ${formatTime(audio.duration)}`;
});

// Seek and volume controls
seek.addEventListener('input', () => { audio.currentTime = seek.value; });
volume.addEventListener('input', () => { audio.volume = volume.value; });

// Auto-play next track when the current one ends
audio.addEventListener('ended', () => { if (currentIndex < tracks.length - 1) loadTrack(currentIndex + 1); });
