/**
 * Sealed interface defining all supported CLI commands with a parser.
 */
package cli;

import java.util.Arrays;
import java.util.stream.Collectors;

/** A CLI command that can be Greet, Calc, or Help. */
public sealed interface Command permits Command.Greet, Command.Calc, Command.Help {

    /** Greets a name, optionally uppercased. */
    record Greet(String name, boolean uppercase) implements Command {}
    /** Performs a binary arithmetic operation. */
    record Calc(Operation op, int a, int b) implements Command {}
    /** Displays usage information. */
    record Help() implements Command {}

    /** Supported arithmetic operations. */
    enum Operation { ADD, SUB, MUL, DIV }

    /** Parses command-line arguments into a Command instance. */
    static Command parse(String... args) {
        if (args.length == 0) return new Help();
        return switch (args[0]) {
            case "greet" -> {
                var name = args.length > 1 ? args[1] : "Welt";
                var upper = args.length > 2 && "--upper".equals(args[2]);
                yield new Greet(name, upper);
            }
            case "calc" -> {
                if (args.length < 4) yield new Help();
                try {
                    var op = Command.Operation.valueOf(args[1].toUpperCase());
                    var a = Integer.parseInt(args[2]);
                    var b = Integer.parseInt(args[3]);
                    yield new Calc(op, a, b);
                } catch (IllegalArgumentException e) {
                    yield new Help();
                }
            }
            default -> new Help();
        };
    }
}
