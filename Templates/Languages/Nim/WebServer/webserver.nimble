[Package]
name        = "webserver"
version     = "0.1.0"
author      = "Your Name"
description = "Async HTTP server in Nim with Jester"
license     = "MIT"

[Dependencies]
requires "jester"

[Tasks]
task run, "Run the web server":
    exec "nim c -r src/webserver.nim"
