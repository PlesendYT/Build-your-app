"""ML training pipeline using scikit-learn."""

from __future__ import annotations

import pandas as pd
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import classification_report
from sklearn.model_selection import train_test_split

from model import create_pipeline


def load_data() -> pd.DataFrame:
    """Create a sample classification dataset."""
    from sklearn.datasets import make_classification

    X, y = make_classification(
        n_samples=1000,
        n_features=10,
        n_informative=5,
        n_redundant=2,
        random_state=42,
    )
    df = pd.DataFrame(X, columns=[f"feature_{i}" for i in range(10)])
    df["target"] = y
    return df


def main() -> None:
    df = load_data()
    X = df.drop(columns="target")
    y = df["target"]

    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42,
    )

    pipeline = create_pipeline()
    pipeline.fit(X_train, y_train)

    y_pred = pipeline.predict(X_test)
    print(classification_report(y_test, y_pred))

    accuracy = (y_pred == y_test).mean()
    print(f"Accuracy: {accuracy:.3f}")


if __name__ == "__main__":
    main()
