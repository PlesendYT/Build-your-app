use anyhow::Result;
use clap::Parser;
use serde::Serialize;
use sysinfo::{Disks, System};

#[derive(Parser)]
#[command(name = "system-tool", about = "System monitoring tool")]
struct Cli {
    #[arg(short, long)]
    json: bool,
    #[command(subcommand)]
    command: Option<Command>,
}

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

#[derive(Serialize)]
struct CpuInfo {
    name: String,
    cores: usize,
    usage: f32,
}

#[derive(Serialize)]
struct MemoryInfo {
    total: u64,
    used: u64,
    free: u64,
}

#[derive(Serialize)]
struct DiskInfo {
    name: String,
    total: u64,
    available: u64,
    mount: String,
}

#[derive(Serialize)]
struct SystemInfo {
    hostname: String,
    os: String,
    kernel: String,
    cpus: Vec<CpuInfo>,
    memory: MemoryInfo,
    disks: Vec<DiskInfo>,
}

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

fn get_memory_info(sys: &System) -> MemoryInfo {
    MemoryInfo {
        total: sys.total_memory(),
        used: sys.used_memory(),
        free: sys.free_memory(),
    }
}

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

fn print_cpu(sys: &mut System) {
    let cpus = get_cpu_info(sys);
    println!("=== CPU Information ===");
    for (i, cpu) in cpus.iter().enumerate() {
        println!("CPU {i}: {} @ {:.1}%", cpu.name, cpu.usage);
    }
}

fn print_memory(sys: &System) {
    let mem = get_memory_info(sys);
    println!("=== Memory ===");
    println!("Total: {}", format_bytes(mem.total));
    println!("Used:  {}", format_bytes(mem.used));
    println!("Free:  {}", format_bytes(mem.free));
}

fn print_disk() {
    let disks = get_disk_info();
    println!("=== Disks ===");
    for disk in &disks {
        println!("{} [{}]: {} / {} available",
            disk.name, disk.mount,
            format_bytes(disk.available), format_bytes(disk.total));
    }
}

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
