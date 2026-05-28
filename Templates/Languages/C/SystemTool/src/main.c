#include <ctype.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>

static long get_cpu_count(void) {
#ifdef _SC_NPROCESSORS_ONLN
    return sysconf(_SC_NPROCESSORS_ONLN);
#else
    return 1;
#endif
}

static void print_cpu_info(void) {
    printf("=== CPU ===\n");
    printf("Cores: %ld\n", get_cpu_count());

    FILE *f = fopen("/proc/cpuinfo", "r");
    if (f) {
        char line[256];
        while (fgets(line, sizeof(line), f)) {
            if (strncmp(line, "model name", 10) == 0) {
                char *colon = strchr(line, ':');
                if (colon) {
                    colon++;
                    while (*colon == ' ') colon++;
                    printf("Model: %s", colon);
                }
                break;
            }
        }
        fclose(f);
    }
}

static void print_memory_info(void) {
    printf("\n=== Memory ===\n");
    FILE *f = fopen("/proc/meminfo", "r");
    if (!f) return;

    char line[256];
    while (fgets(line, sizeof(line), f)) {
        if (strncmp(line, "MemTotal:", 9) == 0 ||
            strncmp(line, "MemAvailable:", 13) == 0) {
            printf("%s", line);
        }
    }
    fclose(f);
}

int main(void) {
    printf("System Tool v1.0\n\n");
    print_cpu_info();
    print_memory_info();
    return 0;
}
