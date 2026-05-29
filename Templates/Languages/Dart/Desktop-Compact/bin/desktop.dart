/// Entry point for the Desktop Compact CLI application.
/// Provides a terminal-based menu for system info, file management, and notes.
import 'dart:io';
import 'package:desktop_compact/desktop.dart';

/// Displays the main menu options in the terminal.
void showMenu() {
  print('''
╔══════════════════════════════════╗
║     Dart Desktop Compact App     ║
╠══════════════════════════════════╣
║  1. System-Info                 ║
║  2. Datei-Manager               ║
║  3. Notizen                     ║
║  4. Beenden                     ║
╚══════════════════════════════════╝''');
}

/// Shows system information obtained from [SystemInfo] and prints it to the console.
void systemInfoMenu() {
  print('System-Informationen:');
  final info = SystemInfo.getInfo();
  info.forEach((key, value) {
    print('  $key: $value');
  });
}

/// Interactive file manager loop that lets users list, create, read, and delete files.
void fileManagerMenu() {
  stdout.write('Verzeichnis-Pfad: ');
  final path = (stdin.readLineSync() ?? '.').trim();
  final fm = FileManager(path);

  while (true) {
    print('\nDatei-Manager:');
    print('  1. Dateien auflisten');
    print('  2. Datei erstellen');
    print('  3. Datei lesen');
    print('  4. Datei löschen');
    print('  5. Zurück');
    stdout.write('Auswahl: ');
    final choice = stdin.readLineSync() ?? '';

    switch (choice) {
      case '1': // List all files in the current directory
        for (final entity in fm.listFiles()) {
          print('  ${entity.statSync().type == FileSystemEntityType.directory ? "[DIR]" : "[FILE]"} ${entity.path}');
        }
      case '2': // Create a new file with user-provided content
        stdout.write('Dateiname: '); final name = stdin.readLineSync() ?? '';
        stdout.write('Inhalt: '); final content = stdin.readLineSync() ?? '';
        fm.createFile(name, content);
      case '3': // Read and display a file's contents
        stdout.write('Dateiname: '); final name = stdin.readLineSync() ?? '';
        final content = fm.readFile(name);
        if (content != null) print(content); else print('Datei nicht gefunden.');
      case '4': // Delete the specified file
        stdout.write('Dateiname: '); final name = stdin.readLineSync() ?? '';
        fm.deleteFile(name);
      case '5': // Return to the main menu
        return;
    }
  }
}

/// Interactive notes management loop for viewing, adding, and deleting notes.
void notesMenu() {
  final notes = NoteApp();
  while (true) {
    print('\nNotizen:');
    print('  1. Alle anzeigen');
    print('  2. Notiz anzeigen');
    print('  3. Notiz hinzufügen');
    print('  4. Notiz löschen');
    print('  5. Zurück');
    stdout.write('Auswahl: ');
    final choice = stdin.readLineSync() ?? '';

    switch (choice) {
      case '1': notes.list(); // List all notes
      case '2': // Show a specific note by index
        stdout.write('Nummer: '); final idx = int.tryParse(stdin.readLineSync() ?? '') ?? 0;
        notes.show(idx - 1);
      case '3': // Add a new note with title and content
        stdout.write('Titel: '); final title = stdin.readLineSync() ?? '';
        stdout.write('Inhalt: '); final content = stdin.readLineSync() ?? '';
        notes.add(title, content);
      case '4': // Delete a note by index
        stdout.write('Nummer: '); final idx = int.tryParse(stdin.readLineSync() ?? '') ?? 0;
        notes.delete(idx - 1);
      case '5': return; // Return to the main menu
    }
  }
}

/// Application entry point. Runs the main menu loop until the user chooses to exit.
void main() {
  while (true) {
    showMenu();
    stdout.write('Auswahl: ');
    final choice = stdin.readLineSync() ?? '';

    switch (choice) {
      case '1': systemInfoMenu();
      case '2': fileManagerMenu();
      case '3': notesMenu();
      case '4': print('Tschüss!'); return;
      default: print('Ungültige Auswahl.');
    }
  }
}
