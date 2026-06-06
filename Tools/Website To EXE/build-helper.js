const fs = require('fs');
const path = require('path');
const { execSync } = require('child_process');

// ── Config laden ────────────────────────────────────────────────────────────
let config;
try {
  config = JSON.parse(fs.readFileSync('config.json', 'utf8'));
} catch (e) {
  console.error('[FEHLER] config.json konnte nicht gelesen werden:', e.message);
  process.exit(1);
}

console.log('App-Name   :', config.app.name);
console.log('Version    :', config.app.version);
console.log('URL        :', config.window.url);
console.log('Portable   :', config.build.portable);
console.log('Installer  :', config.build.installer);
console.log('');

// ── package.json aktualisieren ──────────────────────────────────────────────
let pkg;
try {
  pkg = JSON.parse(fs.readFileSync('package.json', 'utf8'));
} catch (e) {
  console.error('[FEHLER] package.json konnte nicht gelesen werden:', e.message);
  process.exit(1);
}

pkg.name        = config.app.name.toLowerCase().replace(/\s+/g, '-');
pkg.version     = config.app.version;
pkg.description = config.app.description;
pkg.author      = config.app.author;

const targets = [];
if (config.build.portable) targets.push({ target: 'portable', arch: [config.build.targetArch] });
if (config.build.installer) targets.push({ target: 'nsis',     arch: [config.build.targetArch] });

if (targets.length === 0) {
  console.error('[FEHLER] Kein Build-Ziel aktiv! Setze "portable" oder "installer" auf true in config.json.');
  process.exit(1);
}

pkg.build = {
  appId: 'com.' + pkg.name + '.app',
  productName: config.app.name,
  directories: {
    output: config.build.outputDir
  },
  win: {
    target: targets,
    icon: config.app.icon
  }
};

fs.writeFileSync('package.json', JSON.stringify(pkg, null, 2));
console.log('[1/3] package.json aktualisiert.');

// ── npm install (falls node_modules fehlt) ──────────────────────────────────
if (!fs.existsSync('node_modules')) {
  console.log('[2/3] Installiere Abhaengigkeiten (npm install)...');
  console.log('      (Das dauert beim ersten Mal 1-2 Minuten)');
  console.log('');
  try {
    execSync('npm install', { stdio: 'inherit' });
  } catch (e) {
    console.error('[FEHLER] npm install fehlgeschlagen!');
    process.exit(1);
  }
} else {
  console.log('[2/3] Abhaengigkeiten vorhanden, ueberspringe npm install.');
}

// ── electron-builder ────────────────────────────────────────────────────────
console.log('');
console.log('[3/3] Baue App...');
console.log('');
try {
  execSync('npx electron-builder --win', { stdio: 'inherit' });
} catch (e) {
  console.error('[FEHLER] electron-builder fehlgeschlagen!');
  process.exit(1);
}
