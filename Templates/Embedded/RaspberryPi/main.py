"""Raspberry Pi GPIO application: LED control and button input.

Demonstrates basic GPIO operations using RPi.GPIO:
  - Reads a physical push button on BUTTON_PIN.
  - Toggles an LED on LED_PIN each time the button is pressed.
  - Prints the current LED state to the console.
"""

import RPi.GPIO as GPIO
import time

# Pin assignments (BCM numbering)
LED_PIN = 18      # GPIO18, connect an LED + resistor
BUTTON_PIN = 23   # GPIO23, connect a pull-down button

# Shared mutable state for the LED toggle
led_state = False


def setup_gpio():
    """Configure GPIO mode and set up input/output pins."""
    GPIO.setmode(GPIO.BCM)
    GPIO.setup(LED_PIN, GPIO.OUT, initial=GPIO.LOW)
    GPIO.setup(BUTTON_PIN, GPIO.IN, pull_up_down=GPIO.PUD_DOWN)


def button_pressed(channel):
    """Callback executed on the rising edge of the button pin.

    Toggles the LED state and prints the current status.
    """
    global led_state
    led_state = not led_state
    GPIO.output(LED_PIN, GPIO.HIGH if led_state else GPIO.LOW)
    print(f"LED toggled: {'ON' if led_state else 'OFF'}")


def main():
    """Main entry point: set up GPIO and wait for button events."""
    try:
        setup_gpio()
        # Register the rising-edge interrupt on the button pin
        GPIO.add_event_detect(
            BUTTON_PIN, GPIO.RISING,
            callback=button_pressed, bouncetime=200
        )
        print("RPi GPIO ready. Press the button to toggle the LED.")
        # Keep the script alive to receive interrupts
        while True:
            time.sleep(1)
    except KeyboardInterrupt:
        print("\nExiting...")
    finally:
        # Clean up all GPIO resources
        GPIO.cleanup()


if __name__ == "__main__":
    main()
