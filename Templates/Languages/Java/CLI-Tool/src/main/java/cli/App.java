package cli;

import java.util.stream.Stream;

public final class App {

    public static void main(String... args) {
        var cmd = Command.parse(args);
        var result = switch (cmd) {
            case Command.Greet(var name, var upper) -> {
                var msg = "Hallo, " + name + "!";
                yield upper ? msg.toUpperCase() : msg;
            }
            case Command.Calc(var op, var a, var b) -> {
                var res = switch (op) {
                    case ADD -> a + b;
                    case SUB -> a - b;
                    case MUL -> a * b;
                    case DIV -> b != 0 ? a / b : "Fehler: Division durch 0";
                };
                yield "%d %s %d = %s".formatted(a, op.name(), b, res);
            }
            case Command.Help() -> {
                yield Stream.of(
                    "Verwendung: cli <command> [args]",
                    "  greet <name> [--upper]  - Begrüßung",
                    "  calc <op> <a> <b>       - Rechnung (ADD/SUB/MUL/DIV)",
                    "  help                    - Diese Hilfe"
                ).collect(java.util.stream.Collectors.joining("\n"));
            }
        };
        System.out.println(result);
    }
}
