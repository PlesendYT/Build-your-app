// Renderer logic for the Markdown Editor
// Handles text input, live preview rendering, and file I/O integration

let currentFilePath = null;
const editor = document.getElementById('editor');
const preview = document.getElementById('preview');
const fileName = document.getElementById('fileName');

// Simple markdown-to-HTML converter supporting headings, bold, lists, and code
function renderMarkdown(text) {
  return text
    .replace(/^### (.+)/gm, '<h3>$1</h3>')
    .replace(/^## (.+)/gm, '<h2>$1</h2>')
    .replace(/^# (.+)/gm, '<h1>$1</h1>')
    .replace(/\*\*(.+?)\*\*/g, '<strong>$1</strong>')
    .replace(/^- (.+)/gm, '<li>$1</li>')
    .replace(/```([\s\S]*?)```/g, '<pre><code>$1</code></pre>')
    .replace(/\n/g, '<br>');
}

// Update the preview pane in real-time as the user types
editor.addEventListener('input', () => {
  preview.innerHTML = renderMarkdown(editor.value);
});

// Open a file via the IPC bridge and load it into the editor
async function openFile() {
  const result = await window.editorAPI.openFile();
  if (result) {
    editor.value = result.content;
    currentFilePath = result.filePath;
    fileName.textContent = result.filePath.split(/[/\\]/).pop();
    preview.innerHTML = renderMarkdown(editor.value);
  }
}

// Save the current editor content via the IPC bridge
async function saveFile() {
  const result = await window.editorAPI.saveFile(editor.value, currentFilePath);
  if (result) {
    currentFilePath = result;
    fileName.textContent = result.split(/[/\\]/).pop();
  }
}

// Show current file name in the title if defined, otherwise default
function togglePreview() {
  preview.classList.toggle('hidden');
}
