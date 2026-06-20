use clap::{Parser, Subcommand};

#[derive(Parser)]
#[command(name = "demo", version, about = "Demo CLI built with Clap")]
struct Cli {
    #[command(subcommand)]
    command: Cmd,
}

#[derive(Subcommand)]
enum Cmd {
    Greet {
        #[arg(long, default_value = "World")] name: String,
    },
    Add {
        numbers: Vec<f64>,
    },
}

fn main() {
    let cli = Cli::parse();
    match cli.command {
        Cmd::Greet { name } => println!("Hello, {name}!"),
        Cmd::Add { numbers } => {
            let total: f64 = numbers.iter().sum();
            println!("sum = {total}");
        }
    }
}