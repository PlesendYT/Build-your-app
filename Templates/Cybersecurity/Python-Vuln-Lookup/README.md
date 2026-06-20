# Python Vulnerability Lookup Template

> Skelett für ein CVE-Lookup-Skript. / Skeleton for a CVE lookup script.

## Features
- NVD-API-Client / NVD API client
- Optionaler Cache in `cache.json` / Optional cache in `cache.json`
- Einfache CLI mit argparse / Simple CLI with argparse

## How to Use
```bash
cd Python-Vuln-Lookup
pip install -r requirements.txt
python lookup.py CVE-2023-44487
```

## Tech Stack
- Python 3.11+
- requests

## Extension Ideas
- EPSS-Score integrieren / Integrate EPSS score
- Slack-/Teams-Webhook-Benachrichtigung / Slack/Teams webhook notifications
- Bulk-Lookup über CSV / Bulk lookup via CSV