[Package]
name        = "cli-tool"
version     = "0.1.0"
author      = "Your Name"
description = "Modern CLI tool in Nim"
license     = "MIT"

[Tasks]
import os

task run, "Run the CLI tool":
    exec "nim c -r src/cli_tool.nim " & paramStr(1)
