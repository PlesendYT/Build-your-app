package main

import (
	"fmt"
	"os"
	"strconv"

	"github.com/spf13/cobra"
)

var rootCmd = &cobra.Command{
	Use:   "cli",
	Short: "Go CLI-Tool",
	Long:  "Ein modernes CLI-Tool in Go",
	Run: func(cmd *cobra.Command, args []string) {
		cmd.Help()
	},
}

var greetCmd = &cobra.Command{
	Use:   "greet [name]",
	Short: "Begrüßung",
	Args:  cobra.MaximumNArgs(1),
	Run: func(cmd *cobra.Command, args []string) {
		name := "Welt"
		if len(args) > 0 {
			name = args[0]
		}
		upper, _ := cmd.Flags().GetBool("upper")
		msg := fmt.Sprintf("Hallo, %s!", name)
		if upper {
			msg = fmt.Sprintf("%s", msg)
			for i, c := range msg {
				if c >= 'a' && c <= 'z' {
					msg = msg[:i] + string(c-'a'+'A') + msg[i+1:]
				}
			}
		}
		fmt.Println(msg)
	},
}

var calcCmd = &cobra.Command{
	Use:   "calc <op> <a> <b>",
	Short: "Berechnung (add/sub/mul/div)",
	Args:  cobra.ExactArgs(3),
	RunE: func(cmd *cobra.Command, args []string) error {
		op := args[0]
		a, err := strconv.Atoi(args[1])
		if err != nil {
			return fmt.Errorf("ungültige Zahl: %s", args[1])
		}
		b, err := strconv.Atoi(args[2])
		if err != nil {
			return fmt.Errorf("ungültige Zahl: %s", args[2])
		}
		var res int
		switch op {
		case "add":
			res = a + b
		case "sub":
			res = a - b
		case "mul":
			res = a * b
		case "div":
			if b == 0 {
				return fmt.Errorf("division durch 0")
			}
			res = a / b
		default:
			return fmt.Errorf("unbekannte Operation: %s", op)
		}
		fmt.Printf("%d %s %d = %d\n", a, op, b, res)
		return nil
	},
}

func init() {
	greetCmd.Flags().Bool("upper", false, "Großbuchstaben")
	rootCmd.AddCommand(greetCmd)
	rootCmd.AddCommand(calcCmd)
}

func main() {
	if err := rootCmd.Execute(); err != nil {
		fmt.Fprintln(os.Stderr, err)
		os.Exit(1)
	}
}
