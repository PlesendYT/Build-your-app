"""Interactive data exploration in a script-like notebook format."""

from __future__ import annotations

import pandas as pd


def explore() -> None:
    """Interactive data exploration."""
    # Create a sample employee dataset
    df = pd.DataFrame({
        "name": ["Alice", "Bob", "Charlie", "Diana"],
        "age": [25, 30, 35, 28],
        "salary": [50000, 60000, 70000, 55000],
        "department": ["IT", "Sales", "IT", "Sales"],
    })

    # Display dataset summary and basic statistics
    print("=== Dataset ===")
    print(df.to_string(index=False))
    print(f"\nShape: {df.shape}")
    print(f"\nAverage age: {df['age'].mean():.1f}")
    print(f"Average salary: ${df['salary'].mean():,.0f}")

    # Group by department and compute average salary
    grouped = df.groupby("department")["salary"].mean()
    print(f"\nAverage salary by department:\n{grouped}")


if __name__ == "__main__":
    explore()
