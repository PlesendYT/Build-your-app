/// Embedded system simulation demonstrating GPIO, UART, and interrupts in Zig
const std = @import("std");
const builtin = @import("builtin");

/// Supported microcontroller families for comptime dispatch
const ChipType = enum {
    stm32f103,
    stm32f407,
    rp2040,
};

/// Detect target chip from compile-time CPU architecture
const chip: ChipType = comptime blk: {
    if (builtin.target.cpu_arch == .thumb and builtin.target.cpu_model == .cortex_m3) {
        break :blk .stm32f103;
    } else if (builtin.target.cpu_arch == .thumb and builtin.target.cpu_model == .cortex_m4) {
        break :blk .stm32f407;
    } else if (builtin.target.cpu_arch == .thumb and builtin.target.cpu_model == .cortex_m0plus) {
        break :blk .rp2040;
    } else {
        // Host simulation mode
        break :blk .stm32f103;
    }
};

/// Memory-mapped GPIO register layout (simulated)
const GpioRegisters = struct {
    moder: u32,
    otyper: u32,
    ospeedr: u32,
    pupdr: u32,
    idr: u32,
    odr: u32,
    bsrr: u32,
    lckr: u32,
    afrl: u32,
    afrh: u32,
};

/// Global simulated GPIO registers
var gpio_regs: GpioRegisters = @memset(std.mem.zeroes(GpioRegisters));

/// Set the mode of a GPIO pin
fn gpioSetMode(pin: u5, mode: u2) void {
    const shift = pin * 2;
    gpio_regs.moder = (gpio_regs.moder & ~(@as(u32, 0x3) << shift)) | (@as(u32, mode) << shift);
}

/// Write a digital value to a GPIO pin
fn gpioWrite(pin: u5, value: bool) void {
    if (value) {
        gpio_regs.bsrr = @as(u32, 1) << pin;
    } else {
        gpio_regs.bsrr = @as(u32, 1) << (pin + 16);
    }
    gpio_regs.odr = (gpio_regs.odr & ~(@as(u32, 1) << pin)) | (@as(u32, @intFromBool(value)) << pin);
}

/// Read the digital value of a GPIO pin
fn gpioRead(pin: u5) bool {
    return (gpio_regs.idr >> pin) & 1 == 1;
}

/// Get peripheral clock frequency for a given chip at compile time
fn getPeripheralClock(comptime c: ChipType) u32 {
    return switch (c) {
        .stm32f103 => 72_000_000,
        .stm32f407 => 168_000_000,
        .rp2040 => 133_000_000,
    };
}

/// Peripheral clock rate determined at compile time
const PeripheralClock = comptime getPeripheralClock(chip);

/// Simulated UART peripheral
const Uart = struct {
    baud_rate: u32,

    /// Initialize UART with a given baud rate
    fn init(baud: u32) Uart {
        return Uart{ .baud_rate = baud };
    }

    /// Calculate baud rate divisor from peripheral clock
    fn divisor(self: Uart) u32 {
        return PeripheralClock / (16 * self.baud_rate);
    }

    /// Send a single character via UART (writes to stdout in simulation)
    fn sendChar(_: Uart, c: u8) void {
        const stdout = std.io.getStdOut().writer();
        stdout.print("{c}", .{c}) catch {};
    }

    /// Send a string via UART
    fn sendString(self: Uart, str: []const u8) void {
        for (str) |c| {
            self.sendChar(c);
        }
    }
};

/// Simulate an LED blink task on pin 13
fn blinkTask() void {
    gpioSetMode(13, 0b01); // Output mode on pin 13
    var timer: u32 = 0;

    while (true) {
        gpioWrite(13, true);
        delayMs(500);
        gpioWrite(13, false);
        delayMs(500);
        timer += 1;

        if (timer >= 10) {
            break;
        }
    }
}

/// Busy-wait delay in milliseconds
fn delayMs(ms: u32) void {
    const cycles = PeripheralClock / 1000 * ms / 4;
    var i: u32 = 0;
    while (i < cycles) : (i += 1) {
        std.mem.doNotOptimizeAway(i);
    }
}

/// Simulated interrupt handler wrapper
const InterruptHandler = struct {
    handler: *const fn () void,
    enabled: bool,

    /// Trigger the interrupt if enabled
    fn trigger(self: *InterruptHandler) void {
        if (self.enabled) {
            self.handler();
        }
    }
};

/// UART RX interrupt handler instance
var uartRxHandler = InterruptHandler{
    .handler = uartRxCallback,
    .enabled = true,
};

/// Callback executed on UART RX interrupt
fn uartRxCallback() void {
    const stdout = std.io.getStdOut().writer();
    stdout.print("[ISR] UART RX received!\n", .{}) catch {};
}

/// Main entry point demonstrating embedded hardware abstractions
pub fn main() !void {
    const stdout = std.io.getStdOut().writer();

    try stdout.print("Embedded Zig - Chip: {s}\n", .{@tagName(chip)});
    try stdout.print("Peripheral Clock: {d} Hz\n", .{PeripheralClock});

    const uart = Uart.init(115200);
    try stdout.print("UART Divisor: {d}\n", .{uart.divisor()});

    try stdout.print("UART Output: ", .{});
    uart.sendString("Hello from Embedded Zig!\n");

    try stdout.print("GPIO Test: Setting pin 13 high\n", .{});
    gpioSetMode(13, 0b01);
    gpioWrite(13, true);
    try stdout.print("GPIO Pin 13 value: {}\n", .{gpioRead(13)});

    try stdout.print("Triggering UART RX interrupt...\n", .{});
    uartRxHandler.trigger();

    try stdout.print("Running blink simulation...\n", .{});
    blinkTask();

    try stdout.print("Embedded demo complete.\n", .{});

    // Print chip-specific info at comptime
    comptime {
        const info = switch (chip) {
            .stm32f103 => "STM32F103 (Cortex-M3, 72 MHz)",
            .stm32f407 => "STM32F407 (Cortex-M4F, 168 MHz)",
            .rp2040 => "RP2040 (Cortex-M0+, 133 MHz)",
        };
        @setEvalBranchQuota(1000);
        _ = info;
    }
}
