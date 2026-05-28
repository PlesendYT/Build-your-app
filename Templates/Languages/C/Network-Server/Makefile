CC = gcc
CFLAGS = -std=c23 -Wall -Wextra -O2
SRC = src/main.c
TARGET = network-server

.PHONY: all clean
all: $(TARGET)
$(TARGET): $(SRC)
	$(CC) $(CFLAGS) -o $@ $^
clean:
	rm -f $(TARGET)
