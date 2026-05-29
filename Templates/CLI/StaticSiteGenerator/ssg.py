"""Static site generator: converts Markdown files to HTML using templates.

Reads Markdown files from a content directory, renders them through a
Jinja2 template, and outputs a static HTML site.
"""

import sys
import argparse
from pathlib import Path

import markdown
from jinja2 import Environment, FileSystemLoader


def build_site(content_dir, output_dir, template_dir):
    """Convert every .md file in content_dir to .html in output_dir."""
    content_path = Path(content_dir)
    output_path = Path(output_dir)
    output_path.mkdir(parents=True, exist_ok=True)

    # Set up the Jinja2 template environment
    env = Environment(loader=FileSystemLoader(template_dir))
    template = env.get_template("base.html")

    # Process each Markdown file
    md_files = list(content_path.glob("*.md"))
    if not md_files:
        print("No Markdown files found")
        return

    for md_file in md_files:
        html_body = markdown.markdown(md_file.read_text(), extensions=["fenced_code"])
        page_title = md_file.stem.replace("-", " ").title()
        rendered = template.render(title=page_title, content=html_body)

        out_file = output_path / f"{md_file.stem}.html"
        out_file.write_text(rendered)
        print(f"Generated {out_file}")

    print(f"\nSite built: {len(md_files)} pages -> {output_dir}")


def main():
    parser = argparse.ArgumentParser(description="Static Site Generator")
    parser.add_argument("content", help="Directory containing Markdown files")
    parser.add_argument("--output", "-o", default="dist", help="Output directory")
    parser.add_argument(
        "--templates", "-t", default="templates",
        help="Templates directory"
    )
    args = parser.parse_args()

    if not Path(args.content).is_dir():
        print(f"Error: content directory '{args.content}' not found")
        sys.exit(1)
    if not Path(args.templates).is_dir():
        print(f"Error: templates directory '{args.templates}' not found")
        sys.exit(1)

    build_site(args.content, args.output, args.templates)


if __name__ == "__main__":
    main()
