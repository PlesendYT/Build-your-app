CC = gcc
CFLAGS = -std=c23 -Wall -Wextra -O2 $(shell sdl2-config --cflags)
LDFLAGS = $(shell sdl2-config --libs) -lSDL2_ttf -lm
SRC = src/main.c
TARGET = game-sdl2

.PHONY: all clean
all: $(TARGET)
$(TARGET): $(SRC)
	$(CC) $(CFLAGS) -o $@ $^ $(LDFLAGS)
clean:
	rm -f $(TARGET)
