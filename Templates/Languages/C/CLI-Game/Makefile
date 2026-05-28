CC = gcc
CFLAGS = -std=c23 -Wall -Wextra -pedantic -O2
SRC = src/main.c
TARGET = cli-game

.PHONY: all clean
all: $(TARGET)
$(TARGET): $(SRC)
	$(CC) $(CFLAGS) -o $@ $^
clean:
	rm -f $(TARGET)
