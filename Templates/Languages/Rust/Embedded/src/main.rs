//! Embedded Rust template for ARM Cortex-M microcontrollers.
//! A minimal `no_std`, `no_main` program that loops with a delay.

#![no_std]
#![no_main]

use cortex_m::asm;
use cortex_m_rt::entry;
use panic_halt as _;

/// Entry point for the embedded program.
/// Runs an infinite loop with a delay — a placeholder for real firmware.
#[entry]
fn main() -> ! {
    loop {
        asm::delay(8_000_000);
    }
}
