import 'package:cli_tool/cli.dart' as cli;

void main(List<String> args) {
  final command = cli.Command.parse(args);
  print(command.execute());
}
