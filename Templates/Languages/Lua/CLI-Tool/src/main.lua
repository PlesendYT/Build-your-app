#!/usr/bin/env lua

local colors = {
    reset = "\27[0m",
    red = "\27[31m",
    green = "\27[32m",
    yellow = "\27[33m",
    blue = "\27[34m",
    bold = "\27[1m",
}

local function printf(fmt, ...)
    io.write(string.format(fmt, ...))
end

local function error(msg)
    io.stderr:write(colors.red .. "Fehler: " .. msg .. colors.reset .. "\n")
    os.exit(1)
end

local function greet(name, upper)
    name = name or "Welt"
    local msg = "Hallo, " .. name .. "!"
    if upper then
        msg = msg:upper()
    end
    print(colors.green .. msg .. colors.reset)
end

local function calc(op, a, b)
    local result
    if op == "add" then
        result = a + b
    elseif op == "sub" then
        result = a - b
    elseif op == "mul" then
        result = a * b
    elseif op == "div" then
        if b == 0 then error("Division durch 0") end
        result = a / b
    else
        error("Unbekannte Operation: " .. op)
    end
    printf(colors.blue .. "%s %s %s = %s" .. colors.reset .. "\n", a, op, b, result)
end

local function help()
    print(colors.bold .. "Lua CLI-Tool" .. colors.reset)
    print("  greet <name> [--upper]  - Begrüßung")
    print("  calc <op> <a> <b>       - Berechnung (add/sub/mul/div)")
    print("  help                    - Diese Hilfe")
end

local commands = {
    greet = function(args)
        local upper = false
        local name = "Welt"
        for _, arg in ipairs(args) do
            if arg == "--upper" then
                upper = true
            else
                name = arg
            end
        end
        greet(name, upper)
    end,
    calc = function(args)
        if #args < 3 then error("Usage: calc <op> <a> <b>") end
        local op = args[1]
        local a = tonumber(args[2]) or error("Ungültige Zahl: " .. args[2])
        local b = tonumber(args[3]) or error("Ungültige Zahl: " .. args[3])
        calc(op, a, b)
    end,
    help = function() help() end,
}

local args = {...}
local cmd = args[1] or "help"
local cmd_args = {select(2, ...)}

if commands[cmd] then
    commands[cmd](cmd_args)
else
    error("Unbekannter Befehl: " .. cmd)
end
