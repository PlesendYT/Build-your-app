#!/usr/bin/env node

// CLI-Node — command-line tool with greet and todo commands
// Supports: greet --name <name>, todo <list|add|delete>

import { readFile, writeFile } from 'node:fs/promises';
import { resolve } from 'node:path';
import { argv, exit, stdout } from 'node:process';

const args = argv.slice(2);

const HELP = `
CLI-Node — Node.js CLI Tool

Usage:
  node index.js <command> [options]

Commands:
  greet              Prints a greeting
  todo <action>      Manage a todo list (list, add, delete)
  --help, -h         Show this help

Examples:
  node index.js greet --name World
  node index.js todo add "Task 1"
  node index.js todo list
`;

const TODO_PATH = resolve(import.meta.dirname, 'todos.json');

// ── Utility functions ──

/** Wraps text in an ANSI color code */
function color(text, code) {
  return `\x1b[${code}m${text}\x1b[0m`;
}

function green(text) { return color(text, 32); }
function red(text)   { return color(text, 31); }
function bold(text)  { return color(text, 1); }

/** Loads todos from the JSON file, returns an empty array on failure */
async function loadTodos() {
  try {
    const data = await readFile(TODO_PATH, 'utf-8');
    return JSON.parse(data);
  } catch {
    return [];
  }
}

/** Saves todos to the JSON file */
async function saveTodos(todos) {
  await writeFile(TODO_PATH, JSON.stringify(todos, null, 2), 'utf-8');
}

// ── Commands ──

/** Handles the greet command — prints "Hello, <name>!" */
function cmdGreet() {
  const nameIdx = args.indexOf('--name');
  const name = nameIdx !== -1 && args[nameIdx + 1] ? args[nameIdx + 1] : 'World';
  stdout.write(`${green('Hello')}, ${bold(name)}!\n`);
}

/** Handles the todo command — list, add, or delete todos */
async function cmdTodo() {
  const action = args[1];
  const todos = await loadTodos();

  switch (action) {
    case 'list': {
      if (todos.length === 0) {
        stdout.write('No todos.\n');
      } else {
        todos.forEach((t, i) => stdout.write(`${i + 1}. ${t}\n`));
      }
      break;
    }
    case 'add': {
      const text = args.slice(2).join(' ');
      if (!text) { stdout.write(red('Error: No text provided.\n')); exit(1); }
      todos.push(text);
      await saveTodos(todos);
      stdout.write(green(`Todo added: "${text}"\n`));
      break;
    }
    case 'delete': {
      const idx = parseInt(args[2], 10);
      if (isNaN(idx) || idx < 1 || idx > todos.length) {
        stdout.write(red('Invalid number.\n'));
        exit(1);
      }
      const removed = todos.splice(idx - 1, 1);
      await saveTodos(todos);
      stdout.write(green(`Todo deleted: "${removed[0]}"\n`));
      break;
    }
    default:
      stdout.write(red(`Unknown action: "${action}"\n`));
      exit(1);
  }
}

// ── Main ──

/** Entry point — parses the command and dispatches to the appropriate handler */
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
      stdout.write(red(`Unknown command: "${cmd}"\n`));
      stdout.write('Use --help for help.\n');
      exit(1);
  }
}

main().catch(err => {
  stdout.write(red(`Error: ${err.message}\n`));
  exit(1);
});
