local M = {}

-- Default plugin configuration: prefix key, greeting toggle, floating window dimensions
local config = {
    prefix = "<leader>p",
    enable_greeting = true,
    floating_width = 60,
    floating_height = 20,
}

-- Initialize the plugin: merge user options, create commands, keymaps, and autocommands
function M.setup(opts)
    config = vim.tbl_deep_extend("force", config, opts or {})
    M.create_commands()
    M.create_keymaps()
    M.create_autocommands()
end

-- Register user commands: PluginGreet, PluginFloat, PluginStatus
function M.create_commands()
    vim.api.nvim_create_user_command("PluginGreet", function()
        print("Hallo von deinem Plugin!")
    end, {})

    vim.api.nvim_create_user_command("PluginFloat", function()
        M.show_floating_window()
    end, {})

    vim.api.nvim_create_user_command("PluginStatus", function()
        local info = {
            enabled = true,
            config = config,
            version = "1.0.0",
        }
        print(vim.inspect(info))
    end, {})
end

-- Set up key mappings using the configured prefix
function M.create_keymaps()
    vim.keymap.set("n", config.prefix .. "g", "<cmd>PluginGreet<CR>",
        { desc = "Plugin: Begrüßung" })
    vim.keymap.set("n", config.prefix .. "f", "<cmd>PluginFloat<CR>",
        { desc = "Plugin: Float-Fenster" })
    vim.keymap.set("n", config.prefix .. "s", "<cmd>PluginStatus<CR>",
        { desc = "Plugin: Status" })
end

-- Create a VimEnter autocmd that prints a welcome message if enabled
function M.create_autocommands()
    if config.enable_greeting then
        vim.api.nvim_create_autocmd("VimEnter", {
            callback = function()
                vim.schedule(function()
                    print("Plugin geladen! Drücke " .. config.prefix .. "g für Hilfe")
                end)
            end,
        })
    end
end

-- Open a centered floating window with plugin help text
function M.show_floating_window()
    local buf = vim.api.nvim_create_buf(false, true)
    local win = vim.api.nvim_open_win(buf, true, {
        relative = "editor",
        width = config.floating_width,
        height = config.floating_height,
        row = math.floor((vim.o.lines - config.floating_height) / 2),
        col = math.floor((vim.o.columns - config.floating_width) / 2),
        style = "minimal",
        border = "rounded",
    })

    local lines = {
        "╔════════════════════════════════════════════╗",
        "║         Neovim Plugin Demo                 ║",
        "╠════════════════════════════════════════════╣",
        "║  Prefix: " .. config.prefix .. "                  ║",
        "║  g - Begrüßung                            ║",
        "║  f - Float-Fenster (dieses)               ║",
        "║  s - Status                               ║",
        "╚════════════════════════════════════════════╝",
    }

    vim.api.nvim_buf_set_lines(buf, 0, -1, false, lines)
    vim.api.nvim_buf_set_option(buf, "modifiable", false)
    vim.api.nvim_buf_set_keymap(buf, "n", "q", "<cmd>q<CR>", { noremap = true, silent = true })
end

return M
