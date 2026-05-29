import std/[os, parseopt, strutils, times, json]

## Configuration options for the CLI tool
type
  Config* = object
    verbose*: bool
    output*: string

## Custom error type for CLI-specific failures
  CLIError* = object of CatchableError

## Prints the usage guide listing all available commands and options
proc printUsage() =
  echo """
Usage: cli-tool <command> [options]

Commands:
  status              Show system status
  process <file>      Process a file
    -o, --output <file>  Output file
    -v, --verbose        Verbose output
  analyze <dir>       Analyze a directory
    -v, --verbose        Verbose output
"""

## Displays system status including Nim version, host OS, and CPU info
proc cmdStatus() =
  echo "CLI-Tool v1.0.0"
  echo "Nim: ", NimVersion
  echo "Host: ", hostOS
  echo "CPU: ", hostCPU

## Reads and processes a file at the given path, writing output per config
proc cmdProcess(inputPath: string, config: Config) =
  # Read input file, raising a CLIError on failure
  let data = try:
    readFile(inputPath)
  except IOError as e:
    raise newException(CLIError, "Cannot read input: " & inputPath)

  # Build result string with timestamp and byte count
  let result = "[" & now().format("yyyy-MM-dd HH:mm:ss") & "] Processed " &
               $data.len & " bytes from " & inputPath & "\n"

  # Write to output file or stdout depending on config
  if config.output.len > 0:
    writeFile(config.output, result)
  else:
    stdout.write(result)

  # Print verbose details if requested
  if config.verbose:
    echo "Input: ", inputPath
    echo "Size: ", data.len, " bytes"

## Walks a directory and prints file/directory counts and total size
proc cmdAnalyze(dirPath: string, config: Config) =
  var totalSize: int64 = 0
  var fileCount = 0
  var dirCount = 0

  # Walk the directory tree and count files, dirs, and total size
  for kind, path in walkDir(dirPath):
    case kind
    of pcFile:
      fileCount.inc
      totalSize.inc getFileSize(path)
    of pcDir:
      dirCount.inc
    else:
      discard

  echo "Directory: ", dirPath
  echo "Files: ", fileCount
  echo "Directories: ", dirCount
  echo "Total size: ", totalSize, " bytes"

  if config.verbose:
    echo "Detailed scan complete."

when isMainModule:
  var config = Config(verbose: false, output: "")

  # Show usage if no arguments provided
  if paramCount() == 0:
    printUsage()
    quit(0)

  var args = commandLineParams()
  let command = args[0]

  # Dispatch to the appropriate command handler
  case command
  of "status":
    cmdStatus()
  of "process":
    var input = ""
    var i = 1
    # Parse process-specific options
    while i < args.len:
      case args[i]
      of "-o", "--output":
        i.inc
        if i < args.len:
          config.output = args[i]
      of "-v", "--verbose":
        config.verbose = true
      else:
        input = args[i]
      i.inc

    if input.len > 0:
      cmdProcess(input, config)
    else:
      printUsage()
  of "analyze":
    var dir = ""
    var i = 1
    # Parse analyze-specific options
    while i < args.len:
      case args[i]
      of "-v", "--verbose":
        config.verbose = true
      else:
        dir = args[i]
      i.inc

    if dir.len > 0:
      cmdAnalyze(dir, config)
    else:
      printUsage()
  else:
    printUsage()
