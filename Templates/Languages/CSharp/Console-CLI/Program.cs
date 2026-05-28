using System.CommandLine;
using System.Text.Json;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var configOption = new Option<FileInfo?>(
            ["-c", "--config"], "Config file path");
        var verboseOption = new Option<bool>(
            ["-v", "--verbose"], "Verbose output");

        var processCommand = new Command("process", "Process input data")
        {
            new Argument<FileInfo>("input", "Input file"),
            new Option<FileInfo?>(["-o", "--output"], "Output file"),
        };

        var statusCommand = new Command("status", "Show status");

        var rootCommand = new RootCommand("C# CLI Tool")
        {
            configOption, verboseOption, processCommand, statusCommand
        };

        processCommand.SetHandler((input, output) =>
        {
            var content = File.ReadAllText(input.FullName);
            var result = $"Processed {content.Length} bytes";

            if (output is not null)
                File.WriteAllText(output.FullName, result);
            else
                Console.WriteLine(result);
        }, processCommand.Arguments[0], processCommand.Options[1]);

        statusCommand.SetHandler(() =>
        {
            Console.WriteLine($"CLI Tool v1.0");
            Console.WriteLine($"OS: {Environment.OSVersion}");
            Console.WriteLine($".NET: {Environment.Version}");
        });

        return await rootCommand.InvokeAsync(args);
    }
}
