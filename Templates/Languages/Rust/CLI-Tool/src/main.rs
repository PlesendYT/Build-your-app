use anyhow::{Context, Result};
use chrono::Local;
use clap::Parser;
use serde::{Deserialize, Serialize};
use std::path::PathBuf;

#[derive(Parser)]
#[command(name = "cli-tool", version, about = "A modern CLI tool template")]
struct Cli {
    #[arg(short, long, value_name = "FILE")]
    config: Option<PathBuf>,

    #[arg(short, long)]
    verbose: bool,

    #[command(subcommand)]
    command: Option<Command>,
}

#[derive(clap::Subcommand)]
enum Command {
    #[command(about = "Process input data")]
    Process {
        #[arg(required = true)]
        input: PathBuf,
        #[arg(short, long)]
        output: Option<PathBuf>,
    },
    #[command(about = "Show status information")]
    Status,
}

#[derive(Serialize, Deserialize)]
struct Config {
    name: String,
    version: String,
    settings: Settings,
}

#[derive(Serialize, Deserialize)]
struct Settings {
    threads: usize,
    timeout: u64,
}

fn load_config(path: &PathBuf) -> Result<Config> {
    let content = std::fs::read_to_string(path)
        .with_context(|| format!("Failed to read config: {}", path.display()))?;
    let config: Config = serde_json::from_str(&content)
        .with_context(|| "Failed to parse config")?;
    Ok(config)
}

fn process(input: &PathBuf, output: Option<&PathBuf>) -> Result<()> {
    let data = std::fs::read_to_string(input)
        .with_context(|| format!("Cannot read input: {}", input.display()))?;

    let result = format!(
        "[{}] Processed {} bytes from {}\n",
        Local::now().format("%Y-%m-%d %H:%M:%S"),
        data.len(),
        input.display()
    );

    match output {
        Some(p) => std::fs::write(p, &result)
            .with_context(|| format!("Cannot write output: {}", p.display()))?,
        None => print!("{result}"),
    }
    Ok(())
}

fn status() -> Result<()> {
    println!("cli-tool v{}", env!("CARGO_PKG_VERSION"));
    println!("Host: {}", std::env::consts::ARCH);
    println!("OS: {}", std::env::consts::OS);
    println!("Time: {}", Local::now().format("%Y-%m-%d %H:%M:%S"));
    Ok(())
}

fn main() -> Result<()> {
    let cli = Cli::parse();

    if let Some(ref cfg) = cli.config {
        let config = load_config(cfg)?;
        if cli.verbose {
            println!("Loaded config: {} v{}", config.name, config.version);
        }
    }

    match &cli.command {
        Some(Command::Process { input, output }) => process(input, output.as_ref())?,
        Some(Command::Status) => status()?,
        None => {
            println!("Use --help for available commands");
        }
    }
    Ok(())
}
