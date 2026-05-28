"""Flask web application template."""

from __future__ import annotations

from flask import Flask, render_template, request

app = Flask(__name__)


@app.route("/")
def index() -> str:
    """Render the home page."""
    return render_template("index.html", greeting=None)


@app.route("/greet", methods=["POST"])
def greet() -> str:
    """Handle greeting form submission."""
    name = request.form.get("name", "").strip()
    greeting = f"Hello, {name}!" if name else None
    return render_template("index.html", greeting=greeting)


def main() -> None:
    app.run(debug=True)


if __name__ == "__main__":
    main()
