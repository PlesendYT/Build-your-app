package cli;

import java.util.Arrays;
import java.util.stream.Collectors;

public sealed interface Command permits Command.Greet, Command.Calc, Command.Help {

    record Greet(String name, boolean uppercase) implements Command {}
    record Calc(Operation op, int a, int b) implements Command {}
    record Help() implements Command {}

    enum Operation { ADD, SUB, MUL, DIV }

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
