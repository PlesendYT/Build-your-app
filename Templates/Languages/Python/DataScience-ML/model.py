"""ML model definition and pipeline."""

from __future__ import annotations

from sklearn.ensemble import RandomForestClassifier
from sklearn.pipeline import Pipeline
from sklearn.preprocessing import StandardScaler


def create_pipeline() -> Pipeline:
    """Create a machine learning pipeline.

    Returns:
        A scikit-learn Pipeline with scaling and classification.
    """
    # Build a pipeline: first standardize features, then train a random forest
    return Pipeline([
        ("scaler", StandardScaler()),
        ("classifier", RandomForestClassifier(
            n_estimators=100,
            max_depth=10,
            random_state=42,
        )),
    ])
