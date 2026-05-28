[Package]
name        = "gui"
version     = "0.1.0"
author      = "Your Name"
description = "Desktop GUI app in Nim with NiGui"
license     = "MIT"

[Dependencies]
requires "nigui"

[Tasks]
task run, "Run the GUI app":
    exec "nim c -r src/gui.nim"
