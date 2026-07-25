// Renderer-Prozess

const versionSpan = document.getElementById('version');
const platformSpan = document.getElementById('platform');

async function loadInfo() {
  if (window.electronAPI) {
    versionSpan.textContent = await window.electronAPI.getVersion();
    platformSpan.textContent = await window.electronAPI.getPlatform();
  } else {
    versionSpan.textContent = 'N/A (nicht in Electron)';
    platformSpan.textContent = navigator.userAgentData ? navigator.userAgentData.platform : navigator.platform;
  }
}

loadInfo();
