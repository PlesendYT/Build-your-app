CC = gcc
CFLAGS = -std=c23 -Wall -Wextra -O2
LDFLAGS = -lglfw -lGL -lm
SRC = src/main.c
TARGET = graphics-opengl

.PHONY: all clean
all: $(TARGET)
$(TARGET): $(SRC)
	$(CC) $(CFLAGS) -o $@ $^ $(LDFLAGS)
clean:
	rm -f $(TARGET)
