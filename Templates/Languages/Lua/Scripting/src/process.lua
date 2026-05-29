#!/usr/bin/env lua

-- Split a string by a single-character separator into an array
local function split(str, sep)
    local result = {}
    for part in str:gmatch("[^" .. sep .. "]+") do
        table.insert(result, part)
    end
    return result
end

-- Strip leading and trailing whitespace from a string
local function trim(s)
    return s:match("^%s*(.-)%s*$")
end

-- Read a CSV file and return headers array and data array of row-tables
local function parseCSV(filename)
    local file = io.open(filename, "r")
    if not file then
        error("Kann Datei nicht öffnen: " .. filename)
    end
    local lines = {}
    for line in file:lines() do
        table.insert(lines, line)
    end
    file:close()

    if #lines == 0 then return {}, {} end

    local headers = split(lines[1], ",")
    for i, h in ipairs(headers) do headers[i] = trim(h) end

    local data = {}
    for i = 2, #lines do
        local values = split(lines[i], ",")
        local row = {}
        for j, header in ipairs(headers) do
            row[header] = trim(values[j] or "")
        end
        table.insert(data, row)
    end

    return headers, data
end

-- Serialise a Lua value (table, string, number, boolean, nil) to a JSON string
local function toJSON(data, indent)
    indent = indent or 0
    local pad = string.rep("  ", indent)
    local innerPad = string.rep("  ", indent + 1)

    if type(data) == "table" then
        local isArray = true
        for k in pairs(data) do
            if type(k) ~= "number" then
                isArray = false
                break
            end
        end

        local parts = {}
        if isArray then
            for _, v in ipairs(data) do
                table.insert(parts, innerPad .. toJSON(v, indent + 1))
            end
            if #parts == 0 then
                return "[]"
            end
            return "[\n" .. table.concat(parts, ",\n") .. "\n" .. pad .. "]"
        else
            for k, v in pairs(data) do
                local key = '"' .. tostring(k) .. '"'
                table.insert(parts, innerPad .. key .. ": " .. toJSON(v, indent + 1))
            end
            if #parts == 0 then
                return "{}"
            end
            return "{\n" .. table.concat(parts, ",\n") .. "\n" .. pad .. "}"
        end
    elseif type(data) == "string" then
        return '"' .. data:gsub('"', '\\"') .. '"'
    elseif type(data) == "number" then
        return tostring(data)
    elseif type(data) == "boolean" then
        return tostring(data)
    else
        return "null"
    end
end

-- CLI entry point: read CSV, convert to JSON, write output file
local function main()
    local args = {...}
    if #args < 2 then
        print("Usage: lua process.lua <input.csv> <output.json>")
        os.exit(1)
    end

    local inputFile = args[1]
    local outputFile = args[2]

    local headers, data = parseCSV(inputFile)
    print("Gelesene Zeilen: " .. #data)
    print("Spalten: " .. table.concat(headers, ", "))

    local summary = {
        source = inputFile,
        rows = #data,
        columns = headers,
        data = data,
    }

    local json = toJSON(summary)
    local out = io.open(outputFile, "w")
    if not out then
        error("Kann Ausgabedatei nicht schreiben: " .. outputFile)
    end
    out:write(json)
    out:close()
    print("Geschrieben: " .. outputFile)
end

main()
