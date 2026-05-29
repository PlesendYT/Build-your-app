"""Web scraper using requests and BeautifulSoup."""

from __future__ import annotations

from dataclasses import dataclass, field
from urllib.parse import urljoin

import requests
from bs4 import BeautifulSoup


@dataclass
class ScraperResult:
    """Result of a scrape operation."""
    url: str
    title: str | None = None
    links: list[str] = field(default_factory=list)
    text_snippet: str = ""


class WebScraper:
    """Simple web scraper with error handling."""

    def __init__(self, user_agent: str | None = None) -> None:
        # Create a requests session with a custom User-Agent header
        self.session = requests.Session()
        self.session.headers.update({
            "User-Agent": user_agent or (
                "Mozilla/5.0 (compatible; PythonScraper/1.0)"
            ),
        })

    def scrape(self, url: str) -> ScraperResult:
        """Scrape a URL and return structured result."""
        # Fetch the page content
        try:
            response = self.session.get(url, timeout=10)
            response.raise_for_status()
        except requests.RequestException as e:
            return ScraperResult(url=url, text_snippet=f"Error: {e}")

        # Parse HTML with BeautifulSoup
        soup = BeautifulSoup(response.text, "html.parser")
        title = soup.title.string.strip() if soup.title else None

        # Extract up to 20 links, resolving relative URLs
        links = []
        for a_tag in soup.find_all("a", href=True)[:20]:
            href = urljoin(url, a_tag["href"])
            links.append(href)

        # Extract a text snippet (first 300 characters)
        text = soup.get_text(separator=" ", strip=True)
        text_snippet = text[:300] + "..." if len(text) > 300 else text

        return ScraperResult(url=url, title=title, links=links, text_snippet=text_snippet)


def main() -> None:
    url = input("Enter URL to scrape: ").strip() or "https://example.com"
    scraper = WebScraper()
    result = scraper.scrape(url)
    print(f"\n=== Result for {result.url} ===")
    print(f"Title: {result.title}")
    print(f"Links found: {len(result.links)}")
    print(f"Content preview: {result.text_snippet[:200]}")


if __name__ == "__main__":
    main()
