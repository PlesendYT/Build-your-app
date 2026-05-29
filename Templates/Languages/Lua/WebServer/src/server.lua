#!/usr/bin/env lua

local socket = require("socket")
local lfs = require("lfs")

-- TCP server listening on port 8080
local server = socket.tcp()
server:settimeout(0)
server:bind("*", 8080)
server:listen(10)
print("Server läuft auf http://localhost:8080")

-- Parse an HTTP request into method, path, version, and headers
local function parseRequest(data)
    local method, path, version = data:match("^(%w+)%s+(%S+)%s+(HTTP/%d%.%d)")
    local headers = {}
    for key, value in data:gmatch("([%w%-]+):%s*(.+)") do
        headers[key] = value
    end
    return method, path, version, headers
end

-- Build a raw HTTP response string from status code, headers table, and body
local function buildResponse(status, headers, body)
    local response = "HTTP/1.1 " .. status .. "\r\n"
    for k, v in pairs(headers) do
        response = response .. k .. ": " .. v .. "\r\n"
    end
    response = response .. "Content-Length: " .. #body .. "\r\n\r\n" .. body
    return response
end

-- Return a 200 JSON response
local function jsonResponse(data)
    return buildResponse("200 OK", {["Content-Type"] = "application/json"}, data)
end

-- Return a 200 HTML response with UTF-8 charset
local function htmlResponse(body)
    return buildResponse("200 OK", {["Content-Type"] = "text/html; charset=utf-8"}, body)
end

-- Return an error response with the given HTTP code and plain text body
local function errorResponse(code, msg)
    return buildResponse(code, {["Content-Type"] = "text/plain"}, msg)
end

-- Serve a static file from the current directory with MIME type detection
local function serveStatic(path)
    if path == "/" then path = "/index.html" end
    local filepath = "." .. path
    local attr = lfs.attributes(filepath)
    if attr and attr.mode == "file" then
        local file = io.open(filepath, "r")
        if file then
            local content = file:read("*all")
            file:close()
            local ext = path:match("%.(%w+)$") or "txt"
            local mime = "text/plain"
            if ext == "html" or ext == "htm" then mime = "text/html"
            elseif ext == "css" then mime = "text/css"
            elseif ext == "js" then mime = "application/javascript"
            elseif ext == "json" then mime = "application/json"
            elseif ext == "png" then mime = "image/png"
            elseif ext == "jpg" or ext == "jpeg" then mime = "image/jpeg"
            end
            return buildResponse("200 OK", {["Content-Type"] = mime}, content)
        end
    end
    return nil
end

-- Route API requests: /api/hello, /api/time, /api/echo
local function handleAPI(method, path, body)
    if path == "/api/hello" and method == "GET" then
        return jsonResponse('{"message": "Hallo, Welt!", "language": "Lua"}')
    elseif path == "/api/time" and method == "GET" then
        local t = os.date("*t")
        return jsonResponse(string.format(
            '{"year": %d, "month": %d, "day": %d, "hour": %d, "minute": %d}',
            t.year, t.month, t.day, t.hour, t.min))
    elseif path == "/api/echo" and method == "POST" then
        return jsonResponse(string.format('{"echo": %s}', body or "null"))
    end
    return nil
end

-- Read a full HTTP request from a client, route it, and send the response
local function handleClient(client)
    client:settimeout(5)
    local data, err = client:receive("*l")
    if not data then client:close(); return end

    local method, path, version, headers = parseRequest(data)

    -- Read headers
    local headerEnd
    repeat
        data, err = client:receive("*l")
        if data == "" then headerEnd = true end
    until headerEnd or not data

    -- Read body if Content-Length
    local body = ""
    if headers["Content-Length"] then
        body, err = client:receive(tonumber(headers["Content-Length"]))
        body = body or ""
    end

    -- Route
    local response
    if path:match("^/api/") then
        response = handleAPI(method, path, body)
    else
        response = serveStatic(path)
    end

    if not response then
        response = htmlResponse("<h1>Lua WebServer</h1><p>Hallo von Lua!</p>")
    end

    client:send(response)
    client:close()
end

-- Infinite loop: accept connections and spawn client handling
local function acceptLoop()
    while true do
        local client = server:accept()
        if client then
            handleClient(client)
        end
        socket.sleep(0.01)
    end
end

local ok, err = pcall(acceptLoop)
if not ok then
    print("Server-Fehler: " .. tostring(err))
end
server:close()
