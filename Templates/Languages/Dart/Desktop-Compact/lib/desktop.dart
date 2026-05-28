import 'dart:io';
import 'dart:math';

class SystemInfo {
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

class FileManager {
  final String basePath;

  FileManager(this.basePath);

  List<FileSystemEntity> listFiles({bool recursive = false}) {
    final dir = Directory(basePath);
    if (!dir.existsSync()) {
      throw Exception('Verzeichnis existiert nicht: $basePath');
    }
    return dir.listSync(recursive: recursive, followLinks: false);
  }

  void createFile(String name, String content) {
    final file = File('$basePath/$name');
    file.writeAsStringSync(content);
    print('Datei erstellt: ${file.path}');
  }

  String? readFile(String name) {
    final file = File('$basePath/$name');
    if (!file.existsSync()) return null;
    return file.readAsStringSync();
  }

  void deleteFile(String name) {
    final file = File('$basePath/$name');
    if (file.existsSync()) {
      file.deleteSync();
      print('Gelöscht: ${file.path}');
    }
  }
}

class NoteApp {
  final List<Map<String, String>> _notes = [];
  final Random _random = Random();

  NoteApp() {
    _notes.add({'title': 'Willkommen', 'content': 'Dies ist deine erste Notiz.'});
  }

  void add(String title, String content) {
    _notes.add({'title': title, 'content': content});
    print('Notiz hinzugefügt: $title');
  }

  void list() {
    if (_notes.isEmpty) {
      print('Keine Notizen.');
      return;
    }
    for (var i = 0; i < _notes.length; i++) {
      print('${i + 1}. ${_notes[i]['title']}');
    }
  }

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

  void delete(int index) {
    if (index < 0 || index >= _notes.length) {
      print('Ungültiger Index.');
      return;
    }
    final removed = _notes.removeAt(index);
    print('Gelöscht: ${removed['title']}');
  }
}
