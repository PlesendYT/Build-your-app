# AVR/GCC Cross-Compilation
# Install: sudo apt install gcc-avr avr-libc
CC = avr-gcc
CFLAGS = -std=c23 -Wall -Wextra -Os -mmcu=atmega328p
SRC = src/main.c
TARGET = embedded.hex

.PHONY: all clean flash

all: $(TARGET)

$(TARGET): $(SRC)
	$(CC) $(CFLAGS) -o $(TARGET:.hex=.elf) $^
	avr-objcopy -O ihex $(TARGET:.hex=.elf) $@

flash: $(TARGET)
	avrdude -p atmega328p -c arduino -P /dev/ttyUSB0 -b 115200 -U flash:w:$@

clean:
	rm -f $(TARGET:.hex=.elf) $(TARGET)
