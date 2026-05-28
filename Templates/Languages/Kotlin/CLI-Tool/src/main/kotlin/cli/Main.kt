package cli

import kotlinx.coroutines.*
import kotlinx.coroutines.flow.*
import kotlin.system.*

sealed class Command {
    data class Greet(val name: String, val upper: Boolean = false) : Command()
    data class Calc(val op: String, val a: Int, val b: Int) : Command()
    data object Help : Command()
}

fun parseArgs(args: Array<String>): Command = when {
    args.isEmpty() -> Command.Help
    args[0] == "greet" -> {
        val name = args.getOrElse(1) { "Welt" }
        val upper = args.contains("--upper")
        Command.Greet(name, upper)
    }
    args[0] == "calc" && args.size >= 4 -> {
        Command.Calc(args[1], args[2].toIntOrNull() ?: 0, args[3].toIntOrNull() ?: 0)
    }
    else -> Command.Help
}

suspend fun processCommand(cmd: Command): String = coroutineScope {
    flow {
        emit(Unit)
        when (cmd) {
            is Command.Greet -> {
                val msg = "Hallo, ${cmd.name}!"
                emit(if (cmd.upper) msg.uppercase() else msg)
            }
            is Command.Calc -> {
                val res = when (cmd.op) {
                    "add" -> cmd.a + cmd.b
                    "sub" -> cmd.a - cmd.b
                    "mul" -> cmd.a * cmd.b
                    "div" -> if (cmd.b != 0) cmd.a / cmd.b else "Fehler: /0"
                    else -> "Unbekannte Operation: ${cmd.op}"
                }
                emit("${cmd.a} ${cmd.op} ${cmd.b} = $res")
            }
            is Command.Help -> {
                emit(buildString {
                    appendLine("Verwendung: cli <command> [args]")
                    appendLine("  greet <name> [--upper]")
                    appendLine("  calc <op> <a> <b>  (add/sub/mul/div)")
                    appendLine("  help")
                })
            }
        }
    }.last() as String
}

fun main() = runBlocking {
    val cmd = parseArgs(args)
    val result = processCommand(cmd)
    println(result)
}
