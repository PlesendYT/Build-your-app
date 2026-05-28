#include <algorithm>
#include <array>
#include <cstdint>
#include <cstring>
#include <span>

// Simulated embedded GPIO register
struct Gpio {
    volatile uint32_t mode;
    volatile uint32_t output;
    volatile uint32_t input;
};

constexpr auto GPIO_BASE = reinterpret_cast<Gpio*>(0x40020000);
constexpr auto LED_PIN = 13;

auto delay(volatile uint32_t cycles) -> void {
    while (cycles--) {
        __asm__ volatile("nop");
    }
}

auto blink_led(int times, int interval_ms) -> void {
    GPIO_BASE->mode = (GPIO_BASE->mode & ~(3u << (LED_PIN * 2))) | (1u << (LED_PIN * 2));

    for (int i = 0; i < times; ++i) {
        GPIO_BASE->output |= (1u << LED_PIN);
        delay(interval_ms * 1000);
        GPIO_BASE->output &= ~(1u << LED_PIN);
        delay(interval_ms * 1000);
    }
}

auto main() -> int {
    // Application loop
    while (true) {
        blink_led(3, 500);
        delay(2'000'000);
    }
    return 0;
}
