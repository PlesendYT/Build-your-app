import std/[os, parseopt, strutils, times, json]

type
  Config = object
    verbose: bool
    output: string

  CLIError = object of CatchableError

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

proc cmdStatus() =
  echo "CLI-Tool v1.0.0"
  echo "Nim: ", NimVersion
  echo "Host: ", hostOS
  echo "CPU: ", hostCPU

proc cmdProcess(inputPath: string, config: Config) =
  let data = try:
    readFile(inputPath)
  except IOError as e:
    raise newException(CLIError, "Cannot read input: " & inputPath)

  let result = "[" & now().format("yyyy-MM-dd HH:mm:ss") & "] Processed " &
               $data.len & " bytes from " & inputPath & "\n"

  if config.output.len > 0:
    writeFile(config.output, result)
  else:
    stdout.write(result)

  if config.verbose:
    echo "Input: ", inputPath
    echo "Size: ", data.len, " bytes"

proc cmdAnalyze(dirPath: string, config: Config) =
  var totalSize: int64 = 0
  var fileCount = 0
  var dirCount = 0

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

  if paramCount() == 0:
    printUsage()
    quit(0)

  var args = commandLineParams()
  let command = args[0]

  case command
  of "status":
    cmdStatus()
  of "process":
    var input = ""
    var i = 1
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
