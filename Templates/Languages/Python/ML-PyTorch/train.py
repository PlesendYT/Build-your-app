"""Train the TinyClassifier on RandomImageDataset."""
from __future__ import annotations

from pathlib import Path

import torch
from torch import nn, optim
from torch.utils.data import DataLoader

from dataset import RandomImageDataset
from model import TinyClassifier

EPOCHS = 2
BATCH = 32
LR = 1e-3


def main() -> None:
    device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
    model = TinyClassifier().to(device)
    opt = optim.Adam(model.parameters(), lr=LR)
    loss_fn = nn.CrossEntropyLoss()
    loader = DataLoader(RandomImageDataset(), batch_size=BATCH, shuffle=True)

    for epoch in range(EPOCHS):
        model.train()
        running = 0.0
        for x, y in loader:
            x, y = x.to(device), y.to(device)
            opt.zero_grad()
            loss = loss_fn(model(x), y)
            loss.backward(); opt.step()
            running += loss.item() * x.size(0)
        print(f"epoch {epoch+1}: loss={running/len(loader.dataset):.4f}")

    Path("checkpoints").mkdir(exist_ok=True)
    torch.save(model.state_dict(), "checkpoints/tiny.pt")


if __name__ == "__main__":
    main()