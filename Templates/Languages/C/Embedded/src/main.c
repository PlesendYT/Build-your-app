// AVR embedded LED blinker - toggles an LED on PB5 every 500ms.
#include <avr/io.h>
#include <util/delay.h>

#define LED_PIN PB5

int main(void) {
    // Set LED pin as output.
    DDRB |= (1 << LED_PIN);

    // Blink LED indefinitely.
    while (1) {
        PORTB |= (1 << LED_PIN);   // LED on
        _delay_ms(500);
        PORTB &= ~(1 << LED_PIN);  // LED off
        _delay_ms(500);
    }

    return 0;
}
