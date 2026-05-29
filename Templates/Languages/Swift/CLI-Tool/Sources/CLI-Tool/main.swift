/// Entry point for the CLI-Tool application.
/// Defines the root command with subcommands for processing, status, and analysis.
import ArgumentParser
import Foundation

@main
struct CLI: AsyncParsableCommand {
    static let configuration = CommandConfiguration(
        commandName: "cli-tool",
        subcommands: [Process.self, Status.self, Analyze.self],
        defaultSubcommand: Status.self
    )
}

/// Reads an input file, optionally writes the result to an output path.
struct Process: AsyncParsableCommand {
    @Argument(help: "Input file path")
    var input: String

    @Option(name: .shortAndLong, help: "Output file path")
    var output: String?

    @Flag(name: .shortAndLong, help: "Enable verbose output")
    var verbose = false

    func run() async throws {
        let url = URL(fileURLWithPath: input)
        let data = try Data(contentsOf: url)
        let result = "Processed \(data.count) bytes from \(input)"

        // Write to output file or print to console
        if let outputPath = output {
            try result.write(toFile: outputPath, atomically: true, encoding: .utf8)
        } else {
            print(result)
        }

        // Print detailed info when verbose flag is set
        if verbose {
            print("Input: \(input)")
            print("Size: \(data.count) bytes")
        }
    }
}

/// Displays system information such as host name, OS version, and hardware specs.
struct Status: ParsableCommand {
    func run() throws {
        let info = ProcessInfo.processInfo
        print("CLI-Tool v1.0.0")
        print("Host: \(info.hostName)")
        print("OS: \(info.operatingSystemVersionString)")
        print("Processors: \(info.activeProcessorCount)")
        print("Memory: \(info.physicalMemory / 1_073_741_824) GB")
    }
}

/// Recursively analyzes a directory, counting files and total size.
struct Analyze: AsyncParsableCommand {
    @Argument(help: "Directory to analyze")
    var directory: String

    @Flag(name: .shortAndLong, help: "Show detailed information")
    var verbose = false

    func run() async throws {
        let url = URL(fileURLWithPath: directory)
        let fm = FileManager.default

        // Walk the directory tree collecting file metadata
        guard let enumerator = fm.enumerator(at: url, includingPropertiesForKeys: [.fileSizeKey, .isDirectoryKey]) else {
            throw CLIError.invalidDirectory(directory)
        }

        var totalSize: Int64 = 0
        var fileCount = 0
        var dirCount = 0

        for case let fileURL as URL in enumerator {
            let resourceValues = try fileURL.resourceValues(forKeys: [.fileSizeKey, .isDirectoryKey])
            if resourceValues.isDirectory == true {
                dirCount += 1
            } else {
                fileCount += 1
                totalSize += Int64(resourceValues.fileSize ?? 0)
            }
        }

        print("Directory: \(directory)")
        print("Files: \(fileCount)")
        print("Directories: \(dirCount)")
        print("Total size: \(ByteCountFormatter.string(fromByteCount: totalSize, countStyle: .file))")

        if verbose {
            print("\nDetailed scan complete.")
        }
    }
}

/// Errors that can occur during CLI operations.
enum CLIError: Error, CustomStringConvertible {
    case invalidDirectory(String)
    case fileNotFound(String)

    var description: String {
        switch self {
        case .invalidDirectory(let path):
            return "Invalid directory: \(path)"
        case .fileNotFound(let path):
            return "File not found: \(path)"
        }
    }
}
