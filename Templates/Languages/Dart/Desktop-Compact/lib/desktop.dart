/// Library providing system info, file management, and a simple notes app for the Desktop Compact CLI.
import 'dart:io';
import 'dart:math';

/// Collects and returns system-level information about the current environment.
class SystemInfo {
  /// Returns a map of system information (OS, hostname, Dart version, CPUs, PID).
  static Map<String, dynamic> getInfo() {
    return {
      'os': Platform.operatingSystem,
      'os_version': Platform.operatingSystemVersion,
      'host': Platform.localHostname,
      'dart_version': Platform.version,
      'cpus': Platform.numberOfProcessors,
      'pid': Platform.pid,
    };
  }
}

/// Provides file system operations (list, create, read, delete) within a base directory.
class FileManager {
  final String basePath;

  /// Creates a FileManager that operates within the given [basePath].
  FileManager(this.basePath);

  /// Lists all files and directories in the base path.
  /// Optionally lists recursively when [recursive] is true.
  List<FileSystemEntity> listFiles({bool recursive = false}) {
    final dir = Directory(basePath);
    if (!dir.existsSync()) {
      throw Exception('Verzeichnis existiert nicht: $basePath');
    }
    return dir.listSync(recursive: recursive, followLinks: false);
  }

  /// Creates a new file with the given [name] and writes [content] into it.
  void createFile(String name, String content) {
    final file = File('$basePath/$name');
    file.writeAsStringSync(content);
    print('Datei erstellt: ${file.path}');
  }

  /// Reads and returns the content of the file with the given [name], or null if it doesn't exist.
  String? readFile(String name) {
    final file = File('$basePath/$name');
    if (!file.existsSync()) return null;
    return file.readAsStringSync();
  }

  /// Deletes the file with the given [name] if it exists.
  void deleteFile(String name) {
    final file = File('$basePath/$name');
    if (file.existsSync()) {
      file.deleteSync();
      print('Gelöscht: ${file.path}');
    }
  }
}

/// In-memory note-taking app supporting add, list, show, and delete operations.
class NoteApp {
  final List<Map<String, String>> _notes = [];
  final Random _random = Random();

  /// Initializes the note app with a default welcome note.
  NoteApp() {
    _notes.add({'title': 'Willkommen', 'content': 'Dies ist deine erste Notiz.'});
  }

  /// Adds a new note with the given [title] and [content].
  void add(String title, String content) {
    _notes.add({'title': title, 'content': content});
    print('Notiz hinzugefügt: $title');
  }

  /// Lists all note titles with their index numbers.
  void list() {
    if (_notes.isEmpty) {
      print('Keine Notizen.');
      return;
    }
    for (var i = 0; i < _notes.length; i++) {
      print('${i + 1}. ${_notes[i]['title']}');
    }
  }

  /// Displays the full content of the note at the given [index].
  void show(int index) {
    if (index < 0 || index >= _notes.length) {
      print('Ungültiger Index.');
      return;
    }
    final note = _notes[index];
    print('Titel: ${note['title']}');
    print('---');
    print(note['content']);
  }

  /// Removes the note at the given [index] from the list.
  void delete(int index) {
    if (index < 0 || index >= _notes.length) {
      print('Ungültiger Index.');
      return;
    }
    final removed = _notes.removeAt(index);
    print('Gelöscht: ${removed['title']}');
  }
}
