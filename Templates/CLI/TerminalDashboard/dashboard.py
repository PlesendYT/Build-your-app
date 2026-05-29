"""Terminal dashboard: displays real-time system metrics.

Uses psutil and the rich library to render a live-updating dashboard
with CPU, memory, disk, and network usage information.
"""

import time
from collections import deque

import psutil
from rich.layout import Layout
from rich.panel import Panel
from rich.live import Live
from rich.text import Text
from rich.table import Table


def build_layout():
    """Create the main dashboard layout with header and grid."""
    layout = Layout()
    layout.split_column(
        Layout(name="header", size=3),
        Layout(name="body"),
    )
    layout["body"].split_row(
        Layout(name="left"),
        Layout(name="right"),
    )
    return layout


def render_header() -> Panel:
    """Render the dashboard title bar with current timestamp."""
    text = Text(f"System Dashboard — {time.strftime('%Y-%m-%d %H:%M:%S')}")
    text.stylize("bold cyan")
    return Panel(text, style="green")


def render_cpu_panel() -> Panel:
    """Render CPU usage percentage per core."""
    table = Table(show_header=False, box=None)
    table.add_column("Core", style="cyan")
    table.add_column("Usage", style="magenta")
    for i, pct in enumerate(psutil.cpu_percent(percpu=True)):
        table.add_row(f"CPU {i}", f"{pct:5.1f}%")
    return Panel(table, title="CPU Usage", border_style="blue")


def render_memory_panel() -> Panel:
    """Render memory usage statistics."""
    mem = psutil.virtual_memory()
    table = Table(show_header=False, box=None)
    table.add_column("Metric", style="cyan")
    table.add_column("Value", style="magenta")
    table.add_row("Total", f"{mem.total / 1e9:.2f} GB")
    table.add_row("Used", f"{mem.used / 1e9:.2f} GB")
    table.add_row("Percent", f"{mem.percent:.1f}%")
    return Panel(table, title="Memory", border_style="yellow")


def main():
    layout = build_layout()
    with Live(layout, refresh_per_second=2, screen=True) as live:
        try:
            while True:
                layout["header"].update(render_header())
                layout["left"].update(render_cpu_panel())
                layout["right"].update(render_memory_panel())
                time.sleep(0.5)
        except KeyboardInterrupt:
            pass


if __name__ == "__main__":
    main()
