const std = @import("std");

const Config = struct {
    verbose: bool = false,
    output: ?[]const u8 = null,
};

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

    const command = args[1];

    if (std.mem.eql(u8, command, "status")) {
        try cmdStatus();
    } else if (std.mem.eql(u8, command, "process")) {
        var config = Config{};
        var input: ?[]const u8 = null;

        var i: usize = 2;
        while (i < args.len) : (i += 1) {
            if (std.mem.eql(u8, args[i], "-o") or std.mem.eql(u8, args[i], "--output")) {
                i += 1;
                if (i >= args.len) return error.MissingArgument;
                config.output = args[i];
            } else if (std.mem.eql(u8, args[i], "-v") or std.mem.eql(u8, args[i], "--verbose")) {
                config.verbose = true;
            } else {
                input = args[i];
            }
        }

        if (input) |path| {
            try cmdProcess(allocator, path, config);
        } else {
            try printUsage();
        }
    } else if (std.mem.eql(u8, command, "analyze")) {
        var config = Config{};
        var dir: ?[]const u8 = null;

        var i: usize = 2;
        while (i < args.len) : (i += 1) {
            if (std.mem.eql(u8, args[i], "-v") or std.mem.eql(u8, args[i], "--verbose")) {
                config.verbose = true;
            } else {
                dir = args[i];
            }
        }

        if (dir) |path| {
            try cmdAnalyze(allocator, path, config);
        } else {
            try printUsage();
        }
    } else {
        try printUsage();
    }
}

fn printUsage() !void {
    const stdout = std.io.getStdOut().writer();
    try stdout.print(
        \\Usage: cli-tool <command> [options]
        \\
        \\Commands:
        \\  status              Show system status
        \\  process <file>      Process a file
        \\    -o, --output <file>  Output file
        \\    -v, --verbose        Verbose output
        \\  analyze <dir>       Analyze a directory
        \\    -v, --verbose        Verbose output
        \\
    , .{});
}

fn cmdStatus() !void {
    const stdout = std.io.getStdOut().writer();
    try stdout.print("CLI-Tool v1.0.0\n", .{});
    try stdout.print("Zig {}.{}.{}\n", .{
        @import("builtin").zig_version.major,
        @import("builtin").zig_version.minor,
        @import("builtin").zig_version.patch,
    });
}

fn cmdProcess(allocator: std.mem.Allocator, path: []const u8, config: Config) !void {
    const file = std.fs.cwd().openFile(path, .{}) catch |err| {
        std.log.err("Failed to open {s}: {any}", .{ path, err });
        return err;
    };
    defer file.close();

    const data = try file.readToEndAlloc(allocator, 10 * 1024 * 1024);
    defer allocator.free(data);

    const result = try std.fmt.allocPrint(allocator, "Processed {} bytes from {s}\n", .{
        data.len,
        path,
    });
    defer allocator.free(result);

    if (config.output) |out_path| {
        const out_file = try std.fs.cwd().createFile(out_path, .{});
        defer out_file.close();
        try out_file.writeAll(result);
    } else {
        try std.io.getStdOut().writer().print("{s}", .{result});
    }

    if (config.verbose) {
        std.log.info("Input: {s}", .{path});
        std.log.info("Size: {} bytes", .{data.len});
    }
}

fn cmdAnalyze(allocator: std.mem.Allocator, dir_path: []const u8, config: Config) !void {
    var dir = try std.fs.cwd().openDir(dir_path, .{ .iterate = true });
    defer dir.close();

    var walker = try dir.walk(allocator);
    defer walker.deinit();

    var total_size: u64 = 0;
    var file_count: u64 = 0;
    var dir_count: u64 = 0;

    while (try walker.next()) |entry| {
        switch (entry.kind) {
            .file => {
                file_count += 1;
                const stat = try dir.statFile(entry.path);
                total_size += stat.size;
            },
            .directory => {
                dir_count += 1;
            },
            else => {},
        }
    }

    const stdout = std.io.getStdOut().writer();
    try stdout.print("Directory: {s}\n", .{dir_path});
    try stdout.print("Files: {d}\n", .{file_count});
    try stdout.print("Directories: {d}\n", .{dir_count});
    try stdout.print("Total size: {} bytes\n", .{total_size});

    if (config.verbose) {
        try stdout.print("Detailed scan complete.\n", .{});
    }
}
