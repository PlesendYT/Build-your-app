"""Data analysis script using pandas and matplotlib."""

from __future__ import annotations

import matplotlib.pyplot as plt
import pandas as pd
import seaborn as sns


def load_data() -> pd.DataFrame:
    """Create sample dataset for analysis."""
    # Generate 100 days of sales data with weekly seasonality
    return pd.DataFrame({
        "date": pd.date_range("2024-01-01", periods=100, freq="D"),
        "sales": pd.Series(range(100)) + pd.Series(range(100)).apply(
            lambda x: 10 * (x % 7)
        ),
        "category": ["A", "B", "C"] * 33 + ["A"],
    })


def analyze(df: pd.DataFrame) -> None:
    """Perform basic data analysis."""
    # Print dataset info, statistics, and missing value counts
    print("=== Dataset Info ===")
    print(df.info())
    print("\n=== Summary Statistics ===")
    print(df.describe())
    print("\n=== Missing Values ===")
    print(df.isnull().sum())


def visualize(df: pd.DataFrame) -> None:
    """Create data visualizations."""
    sns.set_theme(style="whitegrid")

    # Create a 2x2 grid of plots
    fig, axes = plt.subplots(2, 2, figsize=(12, 8))

    # Top-left: sales trend over time
    axes[0, 0].plot(df["date"], df["sales"])
    axes[0, 0].set_title("Sales Over Time")
    axes[0, 0].tick_params(axis="x", rotation=45)

    # Top-right: total sales by category (bar chart)
    sns.barplot(
        data=df, x="category", y="sales",
        ax=axes[0, 1], estimator="sum",
    )
    axes[0, 1].set_title("Total Sales by Category")

    # Bottom-left: histogram of sales distribution
    sns.histplot(df["sales"], bins=20, ax=axes[1, 0])
    axes[1, 0].set_title("Sales Distribution")

    # Bottom-right: box plot of sales by category
    sns.boxplot(data=df, x="category", y="sales", ax=axes[1, 1])
    axes[1, 1].set_title("Sales by Category")

    plt.tight_layout()
    plt.show()


def main() -> None:
    df = load_data()
    analyze(df)
    visualize(df)


if __name__ == "__main__":
    main()
