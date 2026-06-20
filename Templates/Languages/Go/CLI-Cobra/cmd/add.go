package cmd

import (
    "fmt"
    "strconv"
    "github.com/spf13/cobra"
)

var addCmd = &cobra.Command{
    Use:   "add [numbers...]",
    Short: "Sum numbers",
    Args:  cobra.MinimumNArgs(1),
    Run: func(cmd *cobra.Command, args []string) {
        total := 0.0
        for _, a := range args {
            v, _ := strconv.ParseFloat(a, 64)
            total += v
        }
        fmt.Printf("sum = %v\n", total)
    },
}

func init() { rootCmd.AddCommand(addCmd) }