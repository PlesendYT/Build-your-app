// CLI tool template with command parsing, config loading, and subcommands.
#include <ctype.h>
#include <errno.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>

#ifdef _WIN32
    #include <windows.h>
#else
    #include <unistd.h>
#endif

#define VERSION "1.0.0"
#define MAX_LINE 4096

// Application configuration loaded from a file.
typedef struct {
    char name[256];
    char version[64];
    int threads;
    int timeout;
} Config;

// Parse a key=value config file and populate Config struct.
static int config_load(Config *cfg, const char *path) {
    FILE *f = fopen(path, "r");
    if (!f) return -1;

    char line[MAX_LINE];
    while (fgets(line, sizeof(line), f)) {
        char *eq = strchr(line, '=');
        if (!eq) continue;
        *eq++ = '\0';

        char *key = line;
        char *val = eq;
        key[strcspn(key, " \r\n")] = '\0';
        val[strcspn(val, " \r\n")] = '\0';

        if (strcmp(key, "name") == 0)
            strncpy(cfg->name, val, sizeof(cfg->name) - 1);
        else if (strcmp(key, "version") == 0)
            strncpy(cfg->version, val, sizeof(cfg->version) - 1);
        else if (strcmp(key, "threads") == 0)
            cfg->threads = atoi(val);
        else if (strcmp(key, "timeout") == 0)
            cfg->timeout = atoi(val);
    }
    fclose(f);
    return 0;
}

// "process" command: read input file, optionally write output.
static int cmd_process(int argc, char **argv) {
    if (argc < 1) {
        fprintf(stderr, "Usage: %s process <input> [output]\n", argv[-1]);
        return 1;
    }

    FILE *f = fopen(argv[0], "r");
    if (!f) {
        fprintf(stderr, "Cannot open: %s\n", argv[0]);
        return 1;
    }

    fseek(f, 0, SEEK_END);
    long size = ftell(f);
    fseek(f, 0, SEEK_SET);
    fclose(f);

    if (argc > 1) {
        FILE *out = fopen(argv[1], "w");
        if (out) {
            fprintf(out, "Processed %ld bytes from %s\n", size, argv[0]);
            fclose(out);
        }
    } else {
        printf("Processed %ld bytes from %s\n", size, argv[0]);
    }
    return 0;
}

// "status" command: print tool version and OS info.
static int cmd_status(void) {
    printf("CLI Tool v%s\n", VERSION);
    printf("C Standard: %ld\n", __STDC_VERSION__);
#ifdef __linux__
    printf("OS: Linux\n");
#elif _WIN32
    printf("OS: Windows\n");
#elif __APPLE__
    printf("OS: macOS\n");
#else
    printf("OS: Unknown\n");
#endif
    return 0;
}

// Print usage/help information.
static void print_usage(const char *prog) {
    printf("Usage: %s [options] <command> [args]\n", prog);
    printf("Commands:\n");
    printf("  process <input> [output]  Process a file\n");
    printf("  status                    Show status\n");
    printf("Options:\n");
    printf("  -c, --config <file>      Config file\n");
    printf("  -v, --verbose            Verbose output\n");
    printf("  -h, --help               This help\n");
}

// Entry point: parse options, dispatch to subcommands.
int main(int argc, char **argv) {
    Config cfg = {0};
    int verbose = 0;
    int optind = 1;

    // Parse leading options.
    while (optind < argc && argv[optind][0] == '-') {
        if (strcmp(argv[optind], "-h") == 0 || strcmp(argv[optind], "--help") == 0) {
            print_usage(argv[0]);
            return 0;
        } else if (strcmp(argv[optind], "-v") == 0 || strcmp(argv[optind], "--verbose") == 0) {
            verbose = 1;
        } else if (strcmp(argv[optind], "-c") == 0 || strcmp(argv[optind], "--config") == 0) {
            if (++optind >= argc) {
                fprintf(stderr, "Missing config path\n");
                return 1;
            }
            if (config_load(&cfg, argv[optind]) != 0) {
                fprintf(stderr, "Failed to load config: %s\n", argv[optind]);
                return 1;
            }
            if (verbose) printf("Loaded config: %s v%s\n", cfg.name, cfg.version);
        }
        optind++;
    }

    if (optind >= argc) {
        print_usage(argv[0]);
        return 0;
    }

    if (strcmp(argv[optind], "process") == 0)
        return cmd_process(argc - optind - 1, argv + optind + 1);
    else if (strcmp(argv[optind], "status") == 0)
        return cmd_status();
    else {
        fprintf(stderr, "Unknown command: %s\n", argv[optind]);
        return 1;
    }
}
