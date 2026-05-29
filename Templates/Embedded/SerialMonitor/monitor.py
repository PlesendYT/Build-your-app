"""Python serial monitor with port selection and data logging.

Opens a serial connection to a user-specified port and baud rate,
displays incoming data in real time, and optionally logs it to a file.
"""

import sys
import time
import logging
from datetime import datetime

import serial
import serial.tools.list_ports


def list_available_ports():
    """Return a list of available serial port device names."""
    ports = serial.tools.list_ports.comports()
    return [port.device for port in ports]


def configure_logging(log_filename):
    """Set up file logging for serial data capture."""
    logging.basicConfig(
        filename=log_filename,
        level=logging.INFO,
        format="%(asctime)s - %(message)s",
        datefmt="%Y-%m-%d %H:%M:%S",
    )


def main():
    # Prompt the user to select a serial port from the available list
    ports = list_available_ports()
    if not ports:
        print("No serial ports found. Exiting.")
        sys.exit(1)

    print("Available ports:")
    for i, port in enumerate(ports):
        print(f"  [{i}] {port}")

    port_index = int(input("Select port index: "))
    selected_port = ports[port_index]
    baud_rate = int(input("Enter baud rate (e.g. 9600, 115200): ") or "9600")

    # Optionally enable data logging to a file
    log_enabled = input("Log data to file? (y/N): ").strip().lower() == "y"
    if log_enabled:
        log_file = f"serial_{datetime.now():%Y%m%d_%H%M%S}.log"
        configure_logging(log_file)
        print(f"Logging to {log_file}")

    print(f"Connecting to {selected_port} at {baud_rate} baud...")
    try:
        with serial.Serial(selected_port, baud_rate, timeout=1) as ser:
            print("Connected. Press Ctrl+C to exit.\n")
            while True:
                line = ser.readline().decode("utf-8", errors="replace").strip()
                if line:
                    timestamp = datetime.now().strftime("%H:%M:%S")
                    output = f"[{timestamp}] {line}"
                    print(output)
                    if log_enabled:
                        logging.info(line)
    except serial.SerialException as e:
        print(f"Serial error: {e}")
        sys.exit(1)
    except KeyboardInterrupt:
        print("\nMonitor stopped by user.")


if __name__ == "__main__":
    main()
