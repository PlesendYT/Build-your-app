const std = @import("std");

pub fn main() !void {
    var gpa = std.heap.GeneralPurposeAllocator(.{}){};
    defer _ = gpa.deinit();
    const allocator = gpa.allocator();

    const args = try std.process.argsAlloc(allocator);
    defer std.process.argsFree(allocator, args);

    if (args.len < 2) {
        try printUsage();
        return;
    }

    const mode = args[1];

    if (std.mem.eql(u8, mode, "server")) {
        const port: u16 = if (args.len > 2) try std.fmt.parseInt(u16, args[2], 10) else 8080;
        try runServer(allocator, port);
    } else if (std.mem.eql(u8, mode, "client")) {
        if (args.len < 4) {
            try printUsage();
            return;
        }
        const host = args[2];
        const port = try std.fmt.parseInt(u16, args[3], 10);
        try runClient(allocator, host, port);
    } else {
        try printUsage();
    }
}

fn printUsage() !void {
    const stdout = std.io.getStdOut().writer();
    try stdout.print(
        \\Usage: network <command> [args]
        \\
        \\Commands:
        \\  server [port]          Start a TCP server
        \\  client <host> <port>   Connect to a TCP server
        \\
    , .{});
}

fn runServer(allocator: std.mem.Allocator, port: u16) !void {
    const stdout = std.io.getStdOut().writer();
    try stdout.print("Starting server on port {d}...\n", .{port});

    const address = try std.net.Address.parseIp4("0.0.0.0", port);
    var listener = try address.listen(.{
        .reuse_port = true,
        .reuse_address = true,
    });
    defer listener.dealloc();

    try stdout.print("Server listening on port {d}\n", .{port});

    var client_count: usize = 0;

    while (true) {
        const connection = try listener.accept();
        client_count += 1;
        const client_id = client_count;

        try stdout.print("Client {d} connected: {any}\n", .{ client_id, connection.address });

        const thread = try std.Thread.spawn(.{}, handleClient, .{ allocator, connection, client_id });
        thread.detach();
    }
}

fn handleClient(allocator: std.mem.Allocator, connection: std.net.Server.Connection, client_id: usize) !void {
    defer connection.stream.close();

    var buf: [1024]u8 = undefined;
    const writer = connection.stream.writer();
    const reader = connection.stream.reader();

    writer.writeAll("Welcome to Zig Network Server!\n") catch |err| {
        std.log.err("Client {d}: write error: {any}", .{ client_id, err });
        return;
    };

    while (true) {
        const bytes_read = reader.read(&buf) catch |err| {
            std.log.info("Client {d}: disconnected ({any})", .{ client_id, err });
            return;
        };
        if (bytes_read == 0) {
            std.log.info("Client {d}: closed connection", .{client_id});
            return;
        }

        const message = std.mem.trim(u8, buf[0..bytes_read], &[_]u8{ '\n', '\r' });
        std.log.info("Client {d}: {s}", .{ client_id, message });

        const response = try std.fmt.allocPrint(allocator, "Echo: {s}\n", .{message});
        defer allocator.free(response);

        writer.writeAll(response) catch |err| {
            std.log.err("Client {d}: write error: {any}", .{ client_id, err });
            return;
        };
    }
}

fn runClient(allocator: std.mem.Allocator, host: []const u8, port: u16) !void {
    const stdout = std.io.getStdOut().writer();
    try stdout.print("Connecting to {s}:{d}...\n", .{ host, port });

    const address = try std.net.Address.parseIp4(host, port);
    var stream = try std.net.tcpConnectToAddress(address);
    defer stream.close();

    try stdout.print("Connected!\n", .{});
    try stdout.print("Type messages (empty line to quit):\n", .{});

    const stdin = std.io.getStdIn().reader();
    const writer = stream.writer();
    const reader = stream.reader();

    var buf: [4096]u8 = undefined;

    // Read welcome message
    if (reader.readUntilDelimiterOrEof(&buf, '\n')) |welcome| {
        if (welcome) |msg| {
            try stdout.print("{s}", .{msg});
        }
    } else |_| {}

    while (true) {
        const line = (try stdin.readUntilDelimiterOrEof(&buf, '\n')) orelse break;
        const trimmed = std.mem.trim(u8, line, &[_]u8{ '\n', '\r' });

        if (trimmed.len == 0) break;

        try writer.writeAll(trimmed);
        try writer.writeAll("\n");

        const response = (try reader.readUntilDelimiterOrEof(&buf, '\n')) orelse {
            try stdout.print("Server closed connection\n", .{});
            break;
        };
        try stdout.print("{s}\n", .{response.?});
    }

    try stdout.print("Disconnected.\n", .{});
}
