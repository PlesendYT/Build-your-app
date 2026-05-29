//! Async TCP server/client using Tokio.
//! Supports two modes: a multi-client broadcast server and an interactive client.

use anyhow::{Context, Result};
use clap::Parser;
use tokio::io::{AsyncBufReadExt, AsyncReadExt, AsyncWriteExt, BufReader};
use tokio::net::{TcpListener, TcpStream};
use tokio::sync::broadcast;

/// CLI arguments: choose between server and client mode.
#[derive(Parser)]
#[command(name = "network-async", about = "Async TCP server/client")]
struct Cli {
    #[command(subcommand)]
    command: Command,
}

/// Subcommands for the CLI.
#[derive(clap::Subcommand)]
enum Command {
    /// Start a TCP server that broadcasts messages to all connected clients.
    #[command(about = "Start a TCP server")]
    Server {
        #[arg(default_value = "0.0.0.0:8080")]
        bind: String,
    },
    /// Connect to a running TCP server and exchange messages.
    #[command(about = "Connect to a TCP server")]
    Client {
        #[arg(default_value = "127.0.0.1:8080")]
        address: String,
    },
}

/// Reads lines from a connected client and broadcasts them to all peers.
async fn handle_client(mut stream: TcpStream, tx: broadcast::Sender<String>) -> Result<()> {
    let (reader, mut writer) = stream.split();
    let mut buf_reader = BufReader::new(reader);
    let mut line = String::new();

    loop {
        line.clear();
        let bytes = buf_reader
            .read_line(&mut line)
            .await
            .context("Connection closed")?;

        if bytes == 0 {
            break;
        }

        let msg = line.trim().to_string();
        println!("Received: {msg}");
        tx.send(msg).ok();
    }
    Ok(())
}

/// Binds a TCP listener and spawns a handler for each incoming connection.
async fn run_server(bind: &str) -> Result<()> {
    let listener = TcpListener::bind(bind).await?;
    println!("Server listening on {bind}");
    let (tx, _rx) = broadcast::channel::<String>(100);

    loop {
        let (stream, addr) = listener.accept().await?;
        println!("New connection from {addr}");
        let tx = tx.clone();
        tokio::spawn(async move {
            if let Err(e) = handle_client(stream, tx).await {
                eprintln!("Client error: {e}");
            }
        });
    }
}

/// Connects to a server, reads stdin in a background task, and prints server responses.
async fn run_client(address: &str) -> Result<()> {
    let mut stream = TcpStream::connect(address).await?;
    println!("Connected to {address}");

    let (reader, mut writer) = stream.split();
    let mut buf_reader = BufReader::new(reader);
    let stdin = BufReader::new(tokio::io::stdin());

    // Background task: forward user input to the server.
    tokio::spawn(async move {
        let mut lines = stdin.lines();
        while let Ok(Some(line)) = lines.next_line().await {
            writer.write_all(line.as_bytes()).await.ok();
            writer.write_all(b"\n").await.ok();
        }
    });

    // Main task: print messages received from the server.
    let mut line = String::new();
    loop {
        line.clear();
        match buf_reader.read_line(&mut line).await {
            Ok(0) | Err(_) => break,
            Ok(_) => {
                print!("Server: {}", line.trim());
            }
        }
    }
    Ok(())
}

/// Parse CLI args and dispatch to server or client mode.
#[tokio::main]
async fn main() -> Result<()> {
    let cli = Cli::parse();
    match &cli.command {
        Command::Server { bind } => run_server(bind).await,
        Command::Client { address } => run_client(address).await,
    }
}
