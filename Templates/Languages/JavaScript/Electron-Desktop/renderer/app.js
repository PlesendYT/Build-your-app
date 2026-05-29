// Renderer process — displays system info (app version and platform) in the browser window

const versionSpan = document.getElementById('version');
const platformSpan = document.getElementById('platform');

/** Loads version and platform info from the Electron main process via preload bridge */
async function loadInfo() {
  if (window.electronAPI) {
    versionSpan.textContent = await window.electronAPI.getVersion();
    platformSpan.textContent = await window.electronAPI.getPlatform();
  } else {
    versionSpan.textContent = 'N/A (not in Electron)';
    platformSpan.textContent = navigator.platform;
  }
}

// Initialize on page load
loadInfo();
