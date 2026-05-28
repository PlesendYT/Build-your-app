"""Streamlit data application template."""

from __future__ import annotations

import streamlit as st
import pandas as pd
import numpy as np


st.set_page_config(page_title="Streamlit App", layout="centered")
st.title("Welcome to Streamlit!")
st.markdown("Explore data with interactive widgets.")


name = st.text_input("Enter your name", value="World")
if name:
    st.success(f"Hello, {name}!")


st.subheader("Sample Chart")
chart_data = pd.DataFrame(
    np.random.randn(20, 3),
    columns=["A", "B", "C"],
)
st.line_chart(chart_data)


if st.checkbox("Show raw data"):
    st.dataframe(chart_data)
