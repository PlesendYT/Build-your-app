using System.CommandLine;
using System.Text.Json;

/// <summary>Entry point for the CLI tool using System.CommandLine.</summary>
public class Program
{
    /// <summary>Main entry point. Configures commands and invokes the root command.</summary>
    /// <param name="args">Command-line arguments.</param>
    /// <returns>Exit code.</returns>
    public static async Task<int> Main(string[] args)
    {
        // Define global options
        var configOption = new Option<FileInfo?>(
            ["-c", "--config"], "Config file path");
        var verboseOption = new Option<bool>(
            ["-v", "--verbose"], "Verbose output");

        // Define "process" subcommand with input argument and optional output
        var processCommand = new Command("process", "Process input data")
        {
            new Argument<FileInfo>("input", "Input file"),
            new Option<FileInfo?>(["-o", "--output"], "Output file"),
        };

        // Define "status" subcommand
        var statusCommand = new Command("status", "Show status");

        // Build root command with subcommands and global options
        var rootCommand = new RootCommand("C# CLI Tool")
        {
            configOption, verboseOption, processCommand, statusCommand
        };

        // Handle "process": read file and write or print result
        processCommand.SetHandler((input, output) =>
        {
            var content = File.ReadAllText(input.FullName);
            var result = $"Processed {content.Length} bytes";

            if (output is not null)
                File.WriteAllText(output.FullName, result);
            else
                Console.WriteLine(result);
        }, processCommand.Arguments[0], processCommand.Options[1]);

        // Handle "status": print version and environment info
        statusCommand.SetHandler(() =>
        {
            Console.WriteLine($"CLI Tool v1.0");
            Console.WriteLine($"OS: {Environment.OSVersion}");
            Console.WriteLine($".NET: {Environment.Version}");
        });

        // Run the CLI and return exit code
        return await rootCommand.InvokeAsync(args);
    }
}
