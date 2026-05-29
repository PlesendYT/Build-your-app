"""Example test suite using pytest."""

from __future__ import annotations

import pytest


def add(a: int, b: int) -> int:
    """Simple addition function to test."""
    return a + b


def divide(a: int | float, b: int | float) -> float:
    """Simple division function to test."""
    if b == 0:
        raise ValueError("Cannot divide by zero")
    return a / b


class TestMathOperations:
    """Test group for math functions."""

    def test_add_positive(self) -> None:
        assert add(2, 3) == 5

    def test_add_negative(self) -> None:
        assert add(-1, 1) == 0

    def test_add_zero(self) -> None:
        assert add(0, 0) == 0

    def test_divide_normal(self) -> None:
        assert divide(10, 2) == 5.0

    def test_divide_raises(self) -> None:
        with pytest.raises(ValueError, match="Cannot divide by zero"):
            divide(5, 0)

    @pytest.mark.parametrize(
        "a, b, expected",
        [
            (1, 2, 3),
            (10, 20, 30),
            (-5, 5, 0),
            (100, -50, 50),
        ],
    )
    def test_add_parametrized(self, a: int, b: int, expected: int) -> None:
        assert add(a, b) == expected


@pytest.fixture
def sample_data() -> list[int]:
    """Provide sample data for tests."""
    return [1, 2, 3, 4, 5]


# Fixture-based tests using sample_data
def test_sample_data_sum(sample_data: list[int]) -> None:
    assert sum(sample_data) == 15


def test_sample_data_max(sample_data: list[int]) -> None:
    assert max(sample_data) == 5
