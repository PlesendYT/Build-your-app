[Package]
name        = "systemtool"
version     = "0.1.0"
author      = "Your Name"
description = "System monitoring tool in Nim"
license     = "MIT"

[Tasks]
task run, "Run the system tool":
    exec "nim c -r src/systemtool.nim"
