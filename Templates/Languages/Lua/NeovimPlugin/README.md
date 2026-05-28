# Lua NeovimPlugin

🇩🇪 Neovim-Plugin in Lua mit modernen Neovim-API-Features.

🇬🇧 Neovim plugin in Lua with modern Neovim API features.

## Features
- Benutzerdefinierte Befehle
- Keymaps
- Autocommands
- Floating Window UI
- Benutzerkonfiguration

## How to use
```lua
-- Mit lazy.nvim:
{ "user/plugin", config = function()
  require("plugin").setup({ prefix = "<leader>" })
end }
```

## Tech stack
- Lua 5.1
- Neovim 0.9+ API
- nvim_create_* functions

## Extension ideas
- Add Telescope integration
- Add LSP features
- Add DAP integration
- Add Treesitter queries
