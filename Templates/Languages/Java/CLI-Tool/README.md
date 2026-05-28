# Java CLI-Tool

🇩🇪 Ein modernes Kommandozeilen-Tool in Java 21 mit Pattern Matching, Records und Streams.

🇬🇧 A modern command-line tool in Java 21 with Pattern Matching, Records, and Streams.

## Features
- Argument-Parsing mit Records
- Farbige Ausgabe
- Subcommand-Architektur
- Fehlerbehandlung mit Either-Pattern

## How to use
```bash
mvn package
java -jar target/cli-tool.jar greet --name Welt
java -jar target/cli-tool.jar calc add 5 3
```

## Tech stack
- Java 21
- Maven
- JUnit 5

## Extension ideas
- Add JSON/YAML config loading
- Add HTTP client for API calls
- Add tab completion scripts
