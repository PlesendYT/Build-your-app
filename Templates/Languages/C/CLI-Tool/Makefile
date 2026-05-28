CC = gcc
CFLAGS = -std=c23 -Wall -Wextra -pedantic -O2
LDFLAGS =
SRC = src/main.c
OBJ = $(SRC:.c=.o)
TARGET = cli-tool

.PHONY: all clean

all: $(TARGET)

$(TARGET): $(OBJ)
	$(CC) $(LDFLAGS) -o $@ $^

src/%.o: src/%.c
	$(CC) $(CFLAGS) -c -o $@ $<

clean:
	rm -f $(OBJ) $(TARGET)
