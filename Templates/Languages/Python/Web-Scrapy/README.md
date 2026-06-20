# Scrapy Template

> Web-Crawler mit Scrapy. / Web crawler with Scrapy.

## Features
- Spider-Klasse mit Item-Pipeline / Spider with item pipeline
- Settings für Concurrency und Delay / Settings for concurrency and delay
- CSV-Export / CSV export
- Konfiguration via settings.py / Configuration via settings.py

## How to Use
```bash
cd Web-Scrapy
pip install -r requirements.txt
scrapy crawl quotes -o quotes.csv
```

## Tech Stack
- Python 3.11+
- Scrapy

## Extension Ideas
- Item-Loader mit Input/Output Prozessoren / Item loaders with processors
- Middleware für Proxies und Retries / Middleware for proxies and retries
- Deployment mit scrapyd / Deploy with scrapyd