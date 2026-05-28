// Renderer-Prozess

interface ElectronAPI {
  getVersion: () => Promise<string>;
  getPlatform: () => Promise<string>;
}

declare global {
  interface Window {
    electronAPI?: ElectronAPI;
  }
}

const versionSpan = document.getElementById('version')!;
const platformSpan = document.getElementById('platform')!;

async function loadInfo(): Promise<void> {
  if (window.electronAPI) {
    versionSpan.textContent = await window.electronAPI.getVersion();
    platformSpan.textContent = await window.electronAPI.getPlatform();
  } else {
    versionSpan.textContent = 'N/A (nicht in Electron)';
    platformSpan.textContent = navigator.platform;
  }
}

loadInfo();
