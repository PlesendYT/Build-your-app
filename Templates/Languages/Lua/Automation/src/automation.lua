#!/usr/bin/env lua

local function log(level, msg)
    local ts = os.date("%Y-%m-%d %H:%M:%S")
    io.stderr:write(string.format("[%s] [%s] %s\n", ts, level, msg))
end

local function runCommand(cmd)
    log("INFO", "Ausführung: " .. cmd)
    local handle = io.popen(cmd .. " 2>&1")
    if handle then
        local result = handle:read("*all")
        handle:close()
        if result and #result > 0 then
            log("OUTPUT", result:sub(1, 200))
        end
    else
        log("ERROR", "Konnte Befehl nicht ausführen: " .. cmd)
    end
end

local function watchDirectory(dir, callback)
    log("INFO", "Überwache Verzeichnis: " .. dir)

    local files = {}
    for file in io.popen("ls -la " .. dir .. " 2>/dev/null"):lines() do
        files[file] = true
    end

    while true do
        local current = {}
        local handle = io.popen("ls -la " .. dir .. " 2>/dev/null")
        if handle then
            for file in handle:lines() do
                current[file] = true
                if not files[file] then
                    local filename = file:match("%S+$") or "?"
                    log("CHANGE", "Neue Datei: " .. filename)
                    if callback then callback("create", dir .. "/" .. filename) end
                end
            end
            handle:close()
        end

        for file in pairs(files) do
            if not current[file] then
                local filename = file:match("%S+$") or "?"
                log("CHANGE", "Gelöscht: " .. filename)
                if callback then callback("delete", dir .. "/" .. filename) end
            end
        end

        files = current
        os.execute("sleep 2")
    end
end

local function parseCron(spec)
    local parts = {}
    for part in spec:gmatch("%S+") do
        table.insert(parts, part)
    end
    return parts
end

local function cronMatches(parts, t)
    if t == nil then t = os.date("*t") end

    local function matchPart(part, value)
        if part == "*" then return true end
        if part:match("^%*/(%d+)$") then
            local interval = tonumber(part:match("^%*/(%d+)$"))
            return value % interval == 0
        end
        if tonumber(part) == value then return true end
        return false
    end

    if #parts >= 1 and not matchPart(parts[1], t.min) then return false end
    if #parts >= 2 and not matchPart(parts[2], t.hour) then return false end
    if #parts >= 3 and not matchPart(parts[3], t.day) then return false end
    if #parts >= 4 and not matchPart(parts[4], t.month) then return false end
    if #parts >= 5 and not matchPart(parts[5], t.wday) then return false end
    return true
end

local function runScheduler(spec, cmd)
    local parts = parseCron(spec)
    log("INFO", string.format("Scheduler: %s -> %s", spec, cmd))

    local lastMatch = ""
    while true do
        local t = os.date("*t")
        local check = string.format("%d %d %d %d %d", t.min, t.hour, t.day, t.month, t.wday)
        if check ~= lastMatch and cronMatches(parts) then
            lastMatch = check
            runCommand(cmd)
        end
        os.execute("sleep 30")
    end
end

local function main()
    local args = {...}
    if #args < 2 then
        print("Usage:")
        print("  lua automation.lua watch <directory>")
        print("  lua automation.lua schedule \"<cron>\" <command>")
        print("")
        print("Examples:")
        print('  lua automation.lua watch .')
        print('  lua automation.lua schedule "*/5 * * * *" echo Hallo')
        os.exit(1)
    end

    local mode = args[1]
    if mode == "watch" then
        watchDirectory(args[2], function(event, path)
            if event == "create" then
                runCommand("echo 'Neue Datei: " .. path .. "'")
            end
        end)
    elseif mode == "schedule" then
        local spec = args[2]
        local cmd = table.concat({select(3, ...)}, " ")
        runScheduler(spec, cmd)
    else
        print("Unbekannter Modus: " .. mode)
        os.exit(1)
    end
end

main()
