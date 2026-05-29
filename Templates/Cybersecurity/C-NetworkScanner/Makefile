# Makefile — Build rules for the C network scanner
# Requires libpthread and root privileges for raw socket operations

CC      = gcc
CFLAGS  = -Wall -Wextra -O2 -pthread
TARGET  = scanner
SRCDIR  = src
OBJS    = $(SRCDIR)/main.o

# Default target: link the scanner binary
all: $(TARGET)

# Link object files into the final executable
$(TARGET): $(OBJS)
	$(CC) $(CFLAGS) -o $@ $^

# Compile each .c file into its .o object
$(SRCDIR)/%.o: $(SRCDIR)/%.c $(SRCDIR)/scanner.h
	$(CC) $(CFLAGS) -c $< -o $@

# Remove build artifacts
clean:
	rm -f $(OBJS) $(TARGET)

# Install the binary system-wide (requires sudo)
install: $(TARGET)
	cp $(TARGET) /usr/local/bin/

.PHONY: all clean install
