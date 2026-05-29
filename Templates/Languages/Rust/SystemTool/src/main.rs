//! System monitoring CLI tool.
//! Reports CPU, memory, and disk information using the `sysinfo` crate.

use anyhow::Result;
use clap::Parser;
use serde::Serialize;
use sysinfo::{Disks, System};

/// CLI arguments for the system monitoring tool.
#[derive(Parser)]
#[command(name = "system-tool", about = "System monitoring tool")]
struct Cli {
    #[arg(short, long)]
    json: bool,
    #[command(subcommand)]
    command: Option<Command>,
}

/// Subcommands to select which system information to display.
#[derive(clap::Subcommand)]
enum Command {
    #[command(about = "Show CPU information")]
    Cpu,
    #[command(about = "Show memory information")]
    Memory,
    #[command(about = "Show disk information")]
    Disk,
    #[command(about = "Show all system information")]
    All,
}

/// Serializable CPU information.
#[derive(Serialize)]
struct CpuInfo {
    name: String,
    cores: usize,
    usage: f32,
}

/// Serializable memory information.
#[derive(Serialize)]
struct MemoryInfo {
    total: u64,
    used: u64,
    free: u64,
}

/// Serializable disk information.
#[derive(Serialize)]
struct DiskInfo {
    name: String,
    total: u64,
    available: u64,
    mount: String,
}

/// Aggregated system information for JSON output.
#[derive(Serialize)]
struct SystemInfo {
    hostname: String,
    os: String,
    kernel: String,
    cpus: Vec<CpuInfo>,
    memory: MemoryInfo,
    disks: Vec<DiskInfo>,
}

/// Gathers CPU usage data by refreshing twice (with the minimum interval in between).
fn get_cpu_info(sys: &mut System) -> Vec<CpuInfo> {
    sys.refresh_cpu();
    std::thread::sleep(System::MINIMUM_CPU_UPDATE_INTERVAL);
    sys.refresh_cpu();

    sys.cpus()
        .iter()
        .map(|cpu| CpuInfo {
            name: cpu.name().to_string(),
            cores: 1,
            usage: cpu.cpu_usage(),
        })
        .collect()
}

/// Reads total / used / free memory from the system.
fn get_memory_info(sys: &System) -> MemoryInfo {
    MemoryInfo {
        total: sys.total_memory(),
        used: sys.used_memory(),
        free: sys.free_memory(),
    }
}

/// Lists all mounted disks and their space usage.
fn get_disk_info() -> Vec<DiskInfo> {
    let disks = Disks::new_with_refreshed_list();
    disks
        .iter()
        .map(|d| DiskInfo {
            name: d.name().to_string_lossy().to_string(),
            total: d.total_space(),
            available: d.available_space(),
            mount: d.mount_point().to_string_lossy().to_string(),
        })
        .collect()
}

/// Converts a byte count into a human-readable string (e.g. "1.23 GB").
fn format_bytes(bytes: u64) -> String {
    const UNITS: &[&str] = &["B", "KB", "MB", "GB", "TB"];
    let mut size = bytes as f64;
    let mut unit_idx = 0;
    while size > 1024.0 && unit_idx < UNITS.len() - 1 {
        size /= 1024.0;
        unit_idx += 1;
    }
    format!("{:.2} {}", size, UNITS[unit_idx])
}

/// Prints CPU info for each core.
fn print_cpu(sys: &mut System) {
    let cpus = get_cpu_info(sys);
    println!("=== CPU Information ===");
    for (i, cpu) in cpus.iter().enumerate() {
        println!("CPU {i}: {} @ {:.1}%", cpu.name, cpu.usage);
    }
}

/// Prints total / used / free memory in human-readable form.
fn print_memory(sys: &System) {
    let mem = get_memory_info(sys);
    println!("=== Memory ===");
    println!("Total: {}", format_bytes(mem.total));
    println!("Used:  {}", format_bytes(mem.used));
    println!("Free:  {}", format_bytes(mem.free));
}

/// Prints disk name, mount point, and available space for each disk.
fn print_disk() {
    let disks = get_disk_info();
    println!("=== Disks ===");
    for disk in &disks {
        println!("{} [{}]: {} / {} available",
            disk.name, disk.mount,
            format_bytes(disk.available), format_bytes(disk.total));
    }
}

/// Parse CLI args and dispatch to the requested subcommand.
fn main() -> Result<()> {
    let cli = Cli::parse();
    let mut sys = System::new_all();

    match &cli.command {
        Some(Command::Cpu) => print_cpu(&mut sys),
        Some(Command::Memory) => print_memory(&sys),
        Some(Command::Disk) => print_disk(),
        Some(Command::All) | None => {
            print_cpu(&mut sys);
            println!();
            print_memory(&sys);
            println!();
            print_disk();
        }
    }
    Ok(())
}
