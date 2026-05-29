/// System information tool that reads CPU, memory, disk, and process data from /proc
const std = @import("std");
const builtin = @import("builtin");

/// Main entry point — parses command and dispatches to the appropriate info function
pub fn main() !void {
    var gpa = std.heap.GeneralPurposeAllocator(.{}){};
    defer _ = gpa.deinit();
    const allocator = gpa.allocator();

    const args = try std.process.argsAlloc(allocator);
    defer std.process.argsFree(allocator, args);

    if (args.len > 1) {
        const cmd = args[1];
        if (std.mem.eql(u8, cmd, "cpu")) {
            try printCPU();
        } else if (std.mem.eql(u8, cmd, "memory")) {
            try printMemory();
        } else if (std.mem.eql(u8, cmd, "disk")) {
            try printDisk();
        } else if (std.mem.eql(u8, cmd, "processes")) {
            try printProcesses(allocator);
        } else if (std.mem.eql(u8, cmd, "all")) {
            try printCPU();
            try std.io.getStdOut().writer().print("\n", .{});
            try printMemory();
            try std.io.getStdOut().writer().print("\n", .{});
            try printDisk();
            try std.io.getStdOut().writer().print("\n", .{});
            try printProcesses(allocator);
        } else {
            try printUsage();
        }
    } else {
        try printAll(allocator);
    }
}

/// Print usage instructions
fn printUsage() !void {
    const stdout = std.io.getStdOut().writer();
    try stdout.print(
        \\Usage: system-tool [command]
        \\
        \\Commands:
        \\  cpu         Show CPU info
        \\  memory      Show memory info
        \\  disk        Show disk info
        \\  processes   Show process list
        \\  all         Show everything
        \\
    , .{});
}

/// Print all system information sections
fn printAll(allocator: std.mem.Allocator) !void {
    try printCPU();
    try std.io.getStdOut().writer().print("\n", .{});
    try printMemory();
    try std.io.getStdOut().writer().print("\n", .{});
    try printDisk();
    try std.io.getStdOut().writer().print("\n", .{});
    try printProcesses(allocator);
}

/// Print CPU architecture, core count, and model name from /proc/cpuinfo
fn printCPU() !void {
    const stdout = std.io.getStdOut().writer();
    try stdout.print("=== CPU Information ===\n", .{});
    try stdout.print("Architecture: {s}\n", .{@tagName(builtin.cpu.arch)});
    try stdout.print("Cores: {d}\n", .{std.Thread.getCpuCount() catch 0});

    if (comptime builtin.target.os.tag == .linux) {
        if (std.fs.cwd().openFile("/proc/cpuinfo", .{})) |file| {
            defer file.close();
            var buf: [4096]u8 = undefined;
            const bytes = try file.readAll(&buf);
            const content = buf[0..bytes];
            var iter = std.mem.splitAny(u8, content, "\n");
            while (iter.next()) |line| {
                if (std.mem.startsWith(u8, line, "model name")) {
                    if (std.mem.indexOfScalar(u8, line, ':')) |colon| {
                        try stdout.print("Model: {s}\n", .{std.mem.trim(u8, line[colon + 1 ..], " ")});
                    }
                    break;
                }
            }
        } else |_| {}
    }
}

/// Print memory totals from /proc/meminfo
fn printMemory() !void {
    const stdout = std.io.getStdOut().writer();
    try stdout.print("=== Memory Information ===\n", .{});

    if (comptime builtin.target.os.tag == .linux) {
        if (std.fs.cwd().openFile("/proc/meminfo", .{})) |file| {
            defer file.close();
            var buf: [4096]u8 = undefined;
            const bytes = try file.readAll(&buf);
            const content = buf[0..bytes];
            var iter = std.mem.splitAny(u8, content, "\n");
            while (iter.next()) |line| {
                if (std.mem.startsWith(u8, line, "MemTotal") or
                    std.mem.startsWith(u8, line, "MemFree") or
                    std.mem.startsWith(u8, line, "MemAvailable"))
                {
                    try stdout.print("{s}\n", .{std.mem.trim(u8, line, " ")});
                }
            }
        } else |_| {}
    } else {
        try stdout.print("Memory info not available on this platform\n", .{});
    }
}

/// Print disk I/O statistics from /proc/diskstats
fn printDisk() !void {
    const stdout = std.io.getStdOut().writer();
    try stdout.print("=== Disk Information ===\n", .{});

    if (comptime builtin.target.os.tag == .linux) {
        if (std.fs.cwd().openFile("/proc/diskstats", .{})) |file| {
            defer file.close();
            var buf: [8192]u8 = undefined;
            const bytes = try file.readAll(&buf);
            const content = buf[0..bytes];
            var iter = std.mem.splitAny(u8, content, "\n");
            var count: usize = 0;
            while (iter.next()) |line| : (count += 1) {
                if (count > 10) break;
                var fields = std.mem.tokenizeAny(u8, line, " ");
                _ = fields.next(); // major
                _ = fields.next(); // minor
                if (fields.next()) |name| {
                    _ = fields.next(); // rio
                    _ = fields.next(); // rmerge
                    if (fields.next()) |rsect| {
                        _ = fields.next(); // rtime
                        _ = fields.next(); // wio
                        _ = fields.next(); // wmerge
                        if (fields.next()) |wsect| {
                            try stdout.print("{s}: reads={s} sectors, writes={s} sectors\n", .{ name, rsect, wsect });
                        }
                    }
                }
            }
        } else |_| {}
    } else {
        try stdout.print("Disk info not available on this platform\n", .{});
    }
}

/// Print a list of running processes with PID and name from /proc
fn printProcesses(allocator: std.mem.Allocator) !void {
    const stdout = std.io.getStdOut().writer();
    try stdout.print("=== Process List ===\n", .{});

    if (comptime builtin.target.os.tag == .linux) {
        var dir = try std.fs.cwd().openDir("/proc", .{ .iterate = true });
        defer dir.close();

        var walker = try dir.walk(allocator);
        defer walker.deinit();

        try stdout.print("{s:>8}  {s}\n", .{ "PID", "Name" });
        try stdout.print("--------  ----\n", .{});

        while (try walker.next()) |entry| {
            const pid = std.fmt.parseInt(i32, entry.basename, 10) catch continue;
            const cmdline_path = try std.fs.path.join(allocator, &.{ "/proc", entry.basename, "comm" });
            defer allocator.free(cmdline_path);

            if (std.fs.cwd().openFile(cmdline_path, .{})) |file| {
                defer file.close();
                var buf: [256]u8 = undefined;
                const bytes = try file.readAll(&buf);
                const name = std.mem.trim(u8, buf[0..bytes], &[_]u8{0, '\n', ' '});
                try stdout.print("{d:>8}  {s}\n", .{ pid, name });
            } else |_| {}
        }
    } else {
        try stdout.print("Process list not available on this platform\n", .{});
    }
}
