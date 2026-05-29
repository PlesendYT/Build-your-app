// Simulated embedded LED blinker using GPIO register manipulation (C++).
#include <algorithm>
#include <array>
#include <cstdint>
#include <cstring>
#include <span>

// Simulated memory-mapped GPIO port registers.
struct Gpio {
    volatile uint32_t mode;
    volatile uint32_t output;
    volatile uint32_t input;
};

constexpr auto GPIO_BASE = reinterpret_cast<Gpio*>(0x40020000);
constexpr auto LED_PIN = 13;

// Busy-wait loop using NOP instructions.
auto delay(volatile uint32_t cycles) -> void {
    while (cycles--) {
        __asm__ volatile("nop");
    }
}

// Configure pin as output and blink the LED a given number of times.
auto blink_led(int times, int interval_ms) -> void {
    GPIO_BASE->mode = (GPIO_BASE->mode & ~(3u << (LED_PIN * 2))) | (1u << (LED_PIN * 2));

    for (int i = 0; i < times; ++i) {
        GPIO_BASE->output |= (1u << LED_PIN);
        delay(interval_ms * 1000);
        GPIO_BASE->output &= ~(1u << LED_PIN);
        delay(interval_ms * 1000);
    }
}

// Entry point: blink LED in an infinite loop.
auto main() -> int {
    while (true) {
        blink_led(3, 500);
        delay(2'000'000);
    }
    return 0;
}
