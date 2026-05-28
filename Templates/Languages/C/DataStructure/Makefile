CC = gcc
CFLAGS = -std=c23 -Wall -Wextra -O2 -g
SRC = src/main.c src/datastruct.c
OBJ = $(SRC:.c=.o)
HEADERS = src/datastruct.h
TARGET = datastructure

.PHONY: all clean test

all: $(TARGET)

$(TARGET): $(OBJ) $(HEADERS)
	$(CC) $(CFLAGS) -o $@ $(OBJ)

src/%.o: src/%.c $(HEADERS)
	$(CC) $(CFLAGS) -c -o $@ $<

test: $(TARGET)
	./$(TARGET)

clean:
	rm -f $(OBJ) $(TARGET)
