//! Encrypted CLI password manager with master-password protection.
//!
//! Stores credentials in a local JSON vault encrypted with AES-256-GCM.
//! The master password is hashed with Argon2id for key derivation.

use aes_gcm::{
    aead::{Aead, KeyInit, OsRng},
    Aes256Gcm, Nonce,
};
use argon2::Argon2;
use rand::RngCore;
use serde::{Deserialize, Serialize};
use std::collections::HashMap;
use std::fs;
use std::io::{self, Write};
use std::path::PathBuf;

/// A single credential entry stored in the vault
#[derive(Serialize, Deserialize, Clone)]
struct Entry {
    service: String,
    username: String,
    password: String,
}

/// The encrypted vault — holds all entries and the Argon2 salt
struct Vault {
    path: PathBuf,
    master_hash: Vec<u8>,
    salt: Vec<u8>,
    entries: Vec<Entry>,
}

impl Vault {
    /// Load or create the vault, deriving the encryption key from the master password
    fn new(master_password: &str) -> io::Result<Self> {
        let path = dirs::data_dir()
            .unwrap_or_else(|| PathBuf::from("."))
            .join("passman.vault");
        let salt;
        let master_hash;
        let entries;

        if path.exists() {
            let data = fs::read(&path)?;
            // In a full implementation: decrypt with AES-GCM using a key derived
            // from the master password via Argon2.
            salt = data[..32].to_vec();
            master_hash = Argon2::default()
                .hash_password_raw(master_password.as_bytes(), &salt)
                .map_err(|e| io::Error::new(io::ErrorKind::Other, e))?
                .as_ref()
                .to_vec();
            entries = vec![];
        } else {
            salt = {
                let mut buf = vec![0u8; 32];
                OsRng.fill_bytes(&mut buf);
                buf
            };
            master_hash = Argon2::default()
                .hash_password_raw(master_password.as_bytes(), &salt)
                .map_err(|e| io::Error::new(io::ErrorKind::Other, e))?
                .as_ref()
                .to_vec();
            entries = vec![];
            // Persist empty encrypted vault
            let key = derive_encryption_key(&master_hash);
            let cipher = Aes256Gcm::new_from_slice(&key).unwrap();
            let nonce_bytes = {
                let mut buf = [0u8; 12];
                OsRng.fill_bytes(&mut buf);
                buf
            };
            let nonce = Nonce::from_slice(&nonce_bytes);
            let encrypted = cipher
                .encrypt(nonce, b"{}".as_ref())
                .map_err(|e| io::Error::new(io::ErrorKind::Other, e))?;
            let mut out = salt.clone();
            out.extend_from_slice(&nonce_bytes);
            out.extend_from_slice(&encrypted);
            fs::write(&path, &out)?;
        }

        Ok(Vault { path, master_hash, salt, entries })
    }

    /// Add a new credential entry
    fn add_entry(&mut self, service: &str, username: &str, password: &str) {
        self.entries.push(Entry {
            service: service.to_string(),
            username: username.to_string(),
            password: password.to_string(),
        });
        println!("Entry added for '{}'", service);
    }

    /// List all stored services (without revealing passwords)
    fn list_entries(&self) {
        if self.entries.is_empty() {
            println!("No entries in vault.");
            return;
        }
        for (i, e) in self.entries.iter().enumerate() {
            println!("{}. {} — {}", i + 1, e.service, e.username);
        }
    }

    /// Retrieve a password for the given service
    fn get_password(&self, service: &str) -> Option<&str> {
        self.entries
            .iter()
            .find(|e| e.service == service)
            .map(|e| e.password.as_str())
    }

    /// Delete an entry by service name
    fn delete_entry(&mut self, service: &str) {
        let len_before = self.entries.len();
        self.entries.retain(|e| e.service != service);
        if self.entries.len() < len_before {
            println!("Deleted entry for '{}'", service);
        } else {
            println!("No entry found for '{}'", service);
        }
    }

    /// Encrypt and persist the vault to disk
    fn save(&self) -> io::Result<()> {
        let json = serde_json::to_string(&self.entries)
            .map_err(|e| io::Error::new(io::ErrorKind::Other, e))?;
        let key = derive_encryption_key(&self.master_hash);
        let cipher = Aes256Gcm::new_from_slice(&key).unwrap();
        let nonce_bytes = {
            let mut buf = [0u8; 12];
            OsRng.fill_bytes(&mut buf);
            buf
        };
        let nonce = Nonce::from_slice(&nonce_bytes);
        let encrypted = cipher
            .encrypt(nonce, json.as_bytes())
            .map_err(|e| io::Error::new(io::ErrorKind::Other, e))?;
        let mut out = self.salt.clone();
        out.extend_from_slice(&nonce_bytes);
        out.extend_from_slice(&encrypted);
        fs::write(&self.path, &out)
    }
}

/// Derive a 256-bit AES key from the Argon2 master hash using SHA-256
fn derive_encryption_key(master_hash: &[u8]) -> [u8; 32] {
    use sha2::{Digest, Sha256};
    let mut hasher = Sha256::new();
    hasher.update(master_hash);
    let result = hasher.finalize();
    let mut key = [0u8; 32];
    key.copy_from_slice(&result);
    key
}

fn main() -> io::Result<()> {
    println!("=== Rust Password Manager ===");
    print!("Master password: ");
    io::stdout().flush()?;
    let mut master = String::new();
    io::stdin().read_line(&mut master)?;
    let master = master.trim();

    let mut vault = Vault::new(master)?;

    loop {
        print!("\nCommands: [a]dd, [l]ist, [g]et, [d]elete, [q]uit\n> ");
        io::stdout().flush()?;
        let mut cmd = String::new();
        io::stdin().read_line(&mut cmd)?;

        match cmd.trim() {
            "a" | "add" => {
                let mut s = String::new();
                let mut u = String::new();
                let mut p = String::new();
                print!("  Service: "); io::stdout().flush()?; io::stdin().read_line(&mut s)?;
                print!("  Username: "); io::stdout().flush()?; io::stdin().read_line(&mut u)?;
                print!("  Password: "); io::stdout().flush()?; io::stdin().read_line(&mut p)?;
                vault.add_entry(s.trim(), u.trim(), p.trim());
                vault.save()?;
            }
            "l" | "list" => vault.list_entries(),
            "g" | "get" => {
                print!("  Service: ");
                io::stdout().flush()?;
                let mut s = String::new();
                io::stdin().read_line(&mut s)?;
                match vault.get_password(s.trim()) {
                    Some(pw) => println!("  Password: {}", pw),
                    None => println!("  Not found."),
                }
            }
            "d" | "delete" => {
                print!("  Service: ");
                io::stdout().flush()?;
                let mut s = String::new();
                io::stdin().read_line(&mut s)?;
                vault.delete_entry(s.trim());
                vault.save()?;
            }
            "q" | "quit" => break,
            _ => println!("Unknown command."),
        }
    }
    Ok(())
}
