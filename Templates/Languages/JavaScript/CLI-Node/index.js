#!/usr/bin/env node

import { readFile, writeFile } from 'node:fs/promises';
import { resolve } from 'node:path';
import { argv, exit, stdout } from 'node:process';

const args = argv.slice(2);

const HELP = `
CLI-Node — Node.js CLI Tool

Verwendung / Usage:
  node index.js <command> [options]

Commands:
  greet              Gibt eine Begrüßung aus
  todo <action>      Todo-Liste verwalten (list, add, delete)
  --help, -h         Diese Hilfe anzeigen

Beispiele / Examples:
  node index.js greet --name Welt
  node index.js todo add "Aufgabe 1"
  node index.js todo list
`;

const TODO_PATH = resolve(import.meta.dirname, 'todos.json');

// ── Hilfsfunktionen ──

function color(text, code) {
  return `\x1b[${code}m${text}\x1b[0m`;
}

function green(text) { return color(text, 32); }
function red(text)   { return color(text, 31); }
function bold(text)  { return color(text, 1); }

async function loadTodos() {
  try {
    const data = await readFile(TODO_PATH, 'utf-8');
    return JSON.parse(data);
  } catch {
    return [];
  }
}

async function saveTodos(todos) {
  await writeFile(TODO_PATH, JSON.stringify(todos, null, 2), 'utf-8');
}

// ── Commands ──

function cmdGreet() {
  const nameIdx = args.indexOf('--name');
  const name = nameIdx !== -1 && args[nameIdx + 1] ? args[nameIdx + 1] : 'Welt';
  stdout.write(`${green('Hallo')}, ${bold(name)}!\n`);
}

async function cmdTodo() {
  const action = args[1];
  const todos = await loadTodos();

  switch (action) {
    case 'list': {
      if (todos.length === 0) {
        stdout.write('Keine Todos. / No todos.\n');
      } else {
        todos.forEach((t, i) => stdout.write(`${i + 1}. ${t}\n`));
      }
      break;
    }
    case 'add': {
      const text = args.slice(2).join(' ');
      if (!text) { stdout.write(red('Fehler: Kein Text angegeben.\n')); exit(1); }
      todos.push(text);
      await saveTodos(todos);
      stdout.write(green(`Todo hinzugefügt: "${text}"\n`));
      break;
    }
    case 'delete': {
      const idx = parseInt(args[2], 10);
      if (isNaN(idx) || idx < 1 || idx > todos.length) {
        stdout.write(red('Ungültige Nummer.\n'));
        exit(1);
      }
      const removed = todos.splice(idx - 1, 1);
      await saveTodos(todos);
      stdout.write(green(`Todo gelöscht: "${removed[0]}"\n`));
      break;
    }
    default:
      stdout.write(red(`Unbekannte Aktion: "${action}"\n`));
      exit(1);
  }
}

// ── Main ──

async function main() {
  const cmd = args[0];

  if (!cmd || cmd === '--help' || cmd === '-h') {
    stdout.write(HELP);
    return;
  }

  switch (cmd) {
    case 'greet':
      cmdGreet();
      break;
    case 'todo':
      await cmdTodo();
      break;
    default:
      stdout.write(red(`Unbekannter Befehl: "${cmd}"\n`));
      stdout.write('Verwende --help für Hilfe.\n');
      exit(1);
  }
}

main().catch(err => {
  stdout.write(red(`Fehler: ${err.message}\n`));
  exit(1);
});
