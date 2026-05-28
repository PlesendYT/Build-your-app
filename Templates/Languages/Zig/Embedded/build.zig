const std = @import("std");

pub fn build(b: *std.Build) void {
    const target = b.standardTargetOptions(.{});
    const optimize = b.standardOptimizeOption(.{});

    const exe = b.addExecutable(.{
        .name = "embedded",
        .root_source_file = b.path("src/main.zig"),
        .target = target,
        .optimize = optimize,
    });

    // For actual embedded targets, link with appropriate linker script
    if (target.result.cpu_arch == .arm or target.result.cpu_arch == .thumb) {
        exe.linker_script = b.path("link.ld");
    }

    b.installArtifact(exe);

    const run_cmd = b.addRunArtifact(exe);
    run_cmd.step.dependOn(b.getInstallStep());

    if (b.args) |args| {
        run_cmd.addArgs(args);
    }

    const run_step = b.step("run", "Run the embedded simulation");
    run_step.dependOn(&run_cmd.step);
}
