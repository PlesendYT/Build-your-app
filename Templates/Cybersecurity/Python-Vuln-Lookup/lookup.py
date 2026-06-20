"""Look up a CVE from the NVD API."""
from __future__ import annotations

import argparse
import json
import sys
import urllib.request
from pathlib import Path

CACHE = Path("cache.json")
API = "https://services.nvd.nist.gov/rest/json/cves/2.0"


def fetch(cve_id: str) -> dict:
    if CACHE.exists():
        cache = json.loads(CACHE.read_text(encoding="utf-8"))
        if cve_id in cache:
            return cache[cve_id]
    url = f"{API}?cveId={cve_id}"
    with urllib.request.urlopen(url, timeout=20) as r:
        data = json.loads(r.read().decode("utf-8"))
    cache = json.loads(CACHE.read_text(encoding="utf-8")) if CACHE.exists() else {}
    cache[cve_id] = data
    CACHE.write_text(json.dumps(cache, indent=2), encoding="utf-8")
    return data


def summarize(cve_id: str, data: dict) -> str:
    vulns = data.get("vulnerabilities", [])
    if not vulns:
        return f"{cve_id}: no description"
    descs = vulns[0]["cve"]["descriptions"]
    en = next((d["value"] for d in descs if d["lang"] == "en"), descs[0]["value"])
    return f"{cve_id}\n  {en}"


def main() -> None:
    p = argparse.ArgumentParser()
    p.add_argument("cve", help="CVE id, e.g. CVE-2023-44487")
    args = p.parse_args()
    try:
        data = fetch(args.cve)
    except Exception as e:
        print(f"error: {e}", file=sys.stderr)
        sys.exit(1)
    print(summarize(args.cve, data))


if __name__ == "__main__":
    main()