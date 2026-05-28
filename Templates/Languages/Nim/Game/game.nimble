[Package]
name        = "game"
version     = "0.1.0"
author      = "Your Name"
description = "2D game in Nim with Raylib"
license     = "MIT"

[Dependencies]
requires "raylib_nim"

[Tasks]
task run, "Run the game":
    exec "nim c -r src/game.nim"
