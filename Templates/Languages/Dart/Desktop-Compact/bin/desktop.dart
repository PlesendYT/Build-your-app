import 'dart:io';
import 'package:desktop_compact/desktop.dart';

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

void systemInfoMenu() {
  print('System-Informationen:');
  final info = SystemInfo.getInfo();
  info.forEach((key, value) {
    print('  $key: $value');
  });
}

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
      case '1':
        for (final entity in fm.listFiles()) {
          print('  ${entity.statSync().type == FileSystemEntityType.directory ? "[DIR]" : "[FILE]"} ${entity.path}');
        }
      case '2':
        stdout.write('Dateiname: '); final name = stdin.readLineSync() ?? '';
        stdout.write('Inhalt: '); final content = stdin.readLineSync() ?? '';
        fm.createFile(name, content);
      case '3':
        stdout.write('Dateiname: '); final name = stdin.readLineSync() ?? '';
        final content = fm.readFile(name);
        if (content != null) print(content); else print('Datei nicht gefunden.');
      case '4':
        stdout.write('Dateiname: '); final name = stdin.readLineSync() ?? '';
        fm.deleteFile(name);
      case '5':
        return;
    }
  }
}

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
      case '1': notes.list();
      case '2':
        stdout.write('Nummer: '); final idx = int.tryParse(stdin.readLineSync() ?? '') ?? 0;
        notes.show(idx - 1);
      case '3':
        stdout.write('Titel: '); final title = stdin.readLineSync() ?? '';
        stdout.write('Inhalt: '); final content = stdin.readLineSync() ?? '';
        notes.add(title, content);
      case '4':
        stdout.write('Nummer: '); final idx = int.tryParse(stdin.readLineSync() ?? '') ?? 0;
        notes.delete(idx - 1);
      case '5': return;
    }
  }
}

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
