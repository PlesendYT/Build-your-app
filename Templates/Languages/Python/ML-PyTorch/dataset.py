"""Dummy dataset that yields random tensors."""
from __future__ import annotations

import torch
from torch.utils.data import Dataset


class RandomImageDataset(Dataset):
    def __init__(self, size: int = 512, num_classes: int = 10) -> None:
        self.size = size
        self.num_classes = num_classes

    def __len__(self) -> int:
        return self.size

    def __getitem__(self, idx: int) -> tuple[torch.Tensor, int]:
        x = torch.randn(1, 28, 28)
        y = idx % self.num_classes
        return x, y