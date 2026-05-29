/// Entry point that parses CLI arguments and executes the corresponding command.
import 'package:cli_tool/cli.dart' as cli;

/// Parses the command from [args] and prints the result.
void main(List<String> args) {
  final command = cli.Command.parse(args);
  print(command.execute());
}
