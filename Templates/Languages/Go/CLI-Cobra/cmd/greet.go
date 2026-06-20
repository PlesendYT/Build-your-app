package cmd

import (
    "fmt"
    "github.com/spf13/cobra"
)

var name string

var greetCmd = &cobra.Command{
    Use:   "greet",
    Short: "Print a greeting",
    Run: func(cmd *cobra.Command, args []string) {
        fmt.Printf("Hello, %s!\n", name)
    },
}

func init() {
    rootCmd.AddCommand(greetCmd)
    greetCmd.Flags().StringVar(&name, "name", "World", "name to greet")
}