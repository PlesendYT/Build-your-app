// Package main implements a system monitoring CLI tool that displays
// system info, process lists, and disk usage in JSON format.
package main

import (
	"encoding/json"
	"fmt"
	"os"
	"runtime"
	"time"

	"github.com/shirou/gopsutil/v3/cpu"
	"github.com/shirou/gopsutil/v3/disk"
	"github.com/shirou/gopsutil/v3/load"
	"github.com/shirou/gopsutil/v3/mem"
	"github.com/shirou/gopsutil/v3/process"
)

// main dispatches to the requested subcommand: info, ps, or disk.
func main() {
	if len(os.Args) < 2 {
		fmt.Println("Verwendung: system-tool <command>")
		fmt.Println("  info   - Systeminformationen")
		fmt.Println("  ps     - Prozessliste")
		fmt.Println("  disk   - Festplatteninfo")
		return
	}

	switch os.Args[1] {
	case "info":
		printSysInfo()
	case "ps":
		printProcesses()
	case "disk":
		printDiskInfo()
	default:
		fmt.Printf("Unbekanntes Kommando: %s\n", os.Args[1])
	}
}

// printJSON marshals a value to indented JSON and prints it to stdout.
func printJSON(v interface{}) {
	data, _ := json.MarshalIndent(v, "", "  ")
	fmt.Println(string(data))
}

// printSysInfo gathers CPU, memory, and load information and prints it as JSON.
func printSysInfo() {
	cpuPercent, _ := cpu.Percent(time.Second, false)
	memInfo, _ := mem.VirtualMemory()
	loadAvg, _ := load.Avg()

	info := map[string]interface{}{
		"os":           runtime.GOOS,
		"arch":         runtime.GOARCH,
		"go_version":   runtime.Version(),
		"cpu_cores":    runtime.NumCPU(),
		"cpu_percent":  cpuPercent,
		"memory_total": memInfo.Total,
		"memory_used":  memInfo.Used,
		"memory_pct":   memInfo.UsedPercent,
		"load_avg":     loadAvg,
	}
	printJSON(info)
}

// printProcesses lists up to 20 active processes (with CPU or memory usage)
// and prints them as a JSON array.
func printProcesses() {
	procs, err := process.Processes()
	if err != nil {
		fmt.Fprintf(os.Stderr, "Fehler: %v\n", err)
		return
	}

	type ProcInfo struct {
		PID  int32   `json:"pid"`
		Name string  `json:"name"`
		CPU  float64 `json:"cpu_percent"`
		MEM  float32 `json:"mem_percent"`
	}

	var list []ProcInfo
	for _, p := range procs {
		name, _ := p.Name()
		cpu, _ := p.CPUPercent()
		mem, _ := p.MemoryPercent()
		if cpu > 0 || mem > 0 {
			list = append(list, ProcInfo{PID: p.Pid, Name: name, CPU: cpu, MEM: mem})
		}
		if len(list) >= 20 {
			break
		}
	}
	printJSON(list)
}

// printDiskInfo retrieves partition and usage information and prints it as JSON.
func printDiskInfo() {
	parts, _ := disk.Partitions(false)
	var infos []map[string]interface{}
	for _, p := range parts {
		usage, _ := disk.Usage(p.Mountpoint)
		infos = append(infos, map[string]interface{}{
			"mountpoint": p.Mountpoint,
			"fstype":     p.Fstype,
			"total":      usage.Total,
			"used":       usage.Used,
			"free":       usage.Free,
			"used_pct":   usage.UsedPercent,
		})
	}
	printJSON(infos)
}
