"""Password CLI: generate, store, retrieve, and encrypt passwords.

Uses Fernet symmetric encryption from the cryptography library to
securely store passwords in a local encrypted file. Each entry is
keyed by a service name.
"""

import os
import sys
import json
import base64
import argparse

from cryptography.fernet import Fernet
from cryptography.hazmat.primitives import hashes
from cryptography.hazmat.primitives.kdf.pbkdf2 import PBKDF2HMAC

# Path to the encrypted password store
STORE_PATH = os.path.expanduser("~/.passcli_store")


def derive_key(master_password: str, salt: bytes) -> bytes:
    """Derive a Fernet-compatible key from the master password using PBKDF2."""
    kdf = PBKDF2HMAC(algorithm=hashes.SHA256(), length=32, salt=salt, iterations=100000)
    return base64.urlsafe_b64encode(kdf.derive(master_password.encode()))


def load_store(master_password: str) -> dict:
    """Decrypt and load the password store, returning an empty dict on first use."""
    if not os.path.exists(STORE_PATH):
        return {}
    data = json.loads(open(STORE_PATH).read())
    salt = base64.b64decode(data["salt"])
    key = derive_key(master_password, salt)
    cipher = Fernet(key)
    plaintext = cipher.decrypt(data["vault"].encode())
    return json.loads(plaintext)


def save_store(master_password: str, vault: dict):
    """Encrypt and persist the password store to disk."""
    salt = os.urandom(16)
    key = derive_key(master_password, salt)
    cipher = Fernet(key)
    encrypted = cipher.encrypt(json.dumps(vault).encode())
    payload = json.dumps({"salt": base64.b64encode(salt).decode(), "vault": encrypted.decode()})
    open(STORE_PATH, "w").write(payload)


def cmd_generate(args):
    """Generate a random password of the specified length."""
    import secrets
    import string
    chars = string.ascii_letters + string.digits + string.punctuation
    password = "".join(secrets.choice(chars) for _ in range(args.length))
    print(password)


def cmd_set(args):
    """Store or update a password for a given service."""
    vault = load_store(args.master)
    vault[args.service] = args.password
    save_store(args.master, vault)
    print(f"Password for '{args.service}' saved")


def cmd_get(args):
    """Retrieve the password for a given service."""
    vault = load_store(args.master)
    password = vault.get(args.service)
    if password:
        print(password)
    else:
        print(f"No password found for '{args.service}'")
        sys.exit(1)


def cmd_list(args):
    """List all service names stored in the vault."""
    vault = load_store(args.master)
    if vault:
        print("Stored services:")
        for service in vault:
            print(f"  - {service}")
    else:
        print("No passwords stored")


def main():
    parser = argparse.ArgumentParser(description="Password Manager CLI")
    subparsers = parser.add_subparsers(dest="command", required=True)

    gen = subparsers.add_parser("generate", help="Generate a random password")
    gen.add_argument("--length", type=int, default=24, help="Password length")

    setp = subparsers.add_parser("set", help="Store a password")
    setp.add_argument("--master", required=True, help="Master password")
    setp.add_argument("service", help="Service name")
    setp.add_argument("password", help="Password to store")

    getp = subparsers.add_parser("get", help="Retrieve a password")
    getp.add_argument("--master", required=True, help="Master password")
    getp.add_argument("service", help="Service name")

    lst = subparsers.add_parser("list", help="List stored services")
    lst.add_argument("--master", required=True, help="Master password")

    args = parser.parse_args()
    commands = {
        "generate": cmd_generate,
        "set": cmd_set,
        "get": cmd_get,
        "list": cmd_list,
    }
    commands[args.command](args)


if __name__ == "__main__":
    main()
