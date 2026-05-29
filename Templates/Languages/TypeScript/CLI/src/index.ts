// CLI tool entry point — supports greet and todo commands with persistent JSON storage
import { readFile, writeFile } from 'node:fs/promises';
import { resolve } from 'node:path';
import { argv, exit, stdout } from 'node:process';

// Parse CLI arguments (skip "node" and script path)
const args = argv.slice(2);

// Todo item shape
interface Todo {
  id: number;
  text: string;
  done: boolean;
}

// Help text displayed with --help or -h
const HELP = `
CLI — TypeScript CLI Tool

Verwendung / Usage:
  tsx src/index.ts <command> [options]

Commands:
  greet              Gibt eine Begrüßung aus
  todo <action>      Todo-Liste verwalten (list, add, done)
  --help, -h         Diese Hilfe anzeigen

Beispiele / Examples:
  tsx src/index.ts greet --name Welt
  tsx src/index.ts todo add "Aufgabe 1"
  tsx src/index.ts todo list
`;

// Path to the JSON file that stores todos
const TODO_PATH = resolve(import.meta.dirname, '..', 'todos.json');

// ANSI color helper
function color(text: string, code: number): string {
  return `\x1b[${code}m${text}\x1b[0m`;
}

// Shorthand color/format helpers
const green = (text: string) => color(text, 32);
const red = (text: string) => color(text, 31);
const bold = (text: string) => color(text, 1);

// Load todos from the JSON file, or return an empty array
async function loadTodos(): Promise<Todo[]> {
  try {
    const data = await readFile(TODO_PATH, 'utf-8');
    return JSON.parse(data) as Todo[];
  } catch {
    return [];
  }
}

// Persist todos to the JSON file
async function saveTodos(todos: Todo[]): Promise<void> {
  await writeFile(TODO_PATH, JSON.stringify(todos, null, 2), 'utf-8');
}

// Handle the "greet" command — outputs a coloured greeting
function cmdGreet(): void {
  const nameIdx = args.indexOf('--name');
  const name = nameIdx !== -1 && args[nameIdx + 1] ? args[nameIdx + 1] : 'Welt';
  stdout.write(`${green('Hallo')}, ${bold(name)}!\n`);
}

// Handle the "todo" command — dispatches to list / add / done subcommands
async function cmdTodo(): Promise<void> {
  const action = args[1];
  let todos = await loadTodos();
  let nextId = todos.length > 0 ? Math.max(...todos.map(t => t.id)) + 1 : 1;

  switch (action) {
    case 'list': {
      if (todos.length === 0) {
        stdout.write('Keine Todos. / No todos.\n');
      } else {
        todos.forEach(t => {
          const status = t.done ? '[x]' : '[ ]';
          stdout.write(`${status} ${t.id}: ${t.text}\n`);
        });
      }
      break;
    }
    case 'add': {
      const text = args.slice(2).join(' ');
      if (!text) { stdout.write(red('Fehler: Kein Text angegeben.\n')); exit(1); }
      todos.push({ id: nextId++, text, done: false });
      await saveTodos(todos);
      stdout.write(green(`Todo hinzugefügt: "${text}"\n`));
      break;
    }
    case 'done': {
      const id = parseInt(args[2], 10);
      const todo = todos.find(t => t.id === id);
      if (!todo) { stdout.write(red('Todo nicht gefunden.\n')); exit(1); }
      todo.done = true;
      await saveTodos(todos);
      stdout.write(green(`Todo erledigt: "${todo.text}"\n`));
      break;
    }
    default:
      stdout.write(red(`Unbekannte Aktion: "${action}"\n`));
      exit(1);
  }
}

// Main dispatcher — parse the root command and run the appropriate handler
async function main(): Promise<void> {
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

// Run the CLI and handle top-level errors
main().catch(err => {
  stdout.write(red(`Fehler: ${(err as Error).message}\n`));
  exit(1);
});
