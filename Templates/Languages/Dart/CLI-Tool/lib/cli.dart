import 'dart:io';
import 'package:args/args.dart';

sealed class Command {
  factory Command.parse(List<String> args) {
    final parser = ArgParser()
      ..addFlag('upper', abbr: 'u', help: 'Großbuchstaben');

    if (args.isEmpty) return HelpCommand();

    switch (args[0]) {
      case 'greet':
        final result = parser.parse(args.skip(1).toList());
        final name = result.rest.isNotEmpty ? result.rest.first : 'Welt';
        return GreetCommand(name, result['upper'] as bool);
      case 'calc':
        if (args.length < 4) return HelpCommand();
        final op = args[1];
        final a = int.tryParse(args[2]) ?? 0;
        final b = int.tryParse(args[3]) ?? 0;
        return CalcCommand(op, a, b);
      default:
        return HelpCommand();
    }
  }

  String execute();
}

class GreetCommand implements Command {
  final String name;
  final bool upper;
  GreetCommand(this.name, this.upper);

  @override
  String execute() {
    var msg = 'Hallo, $name!';
    return upper ? msg.toUpperCase() : msg;
  }
}

class CalcCommand implements Command {
  final String op;
  final int a;
  final int b;
  CalcCommand(this.op, this.a, this.b);

  @override
  String execute() {
    final result = switch (op) {
      'add' => a + b,
      'sub' => a - b,
      'mul' => a * b,
      'div' => b != 0 ? a / b : 'Fehler: /0',
      _ => 'Unbekannte Operation: $op',
    };
    return '$a $op $b = $result';
  }
}

class HelpCommand implements Command {
  @override
  String execute() {
    return '''
Verwendung: cli <command> [args]
  greet <name> [--upper]  - Begrüßung
  calc <op> <a> <b>       - Berechnung
  help                    - Diese Hilfe''';
  }
}
