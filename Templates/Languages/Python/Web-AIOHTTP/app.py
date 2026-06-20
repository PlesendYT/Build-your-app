"""Minimal aiohttp server template."""
from __future__ import annotations

from aiohttp import web

app = web.Application()
_items: dict[int, dict] = {}
_next_id = 1


async def list_items(_: web.Request) -> web.Response:
    return web.json_response(list(_items.values()))


async def create_item(request: web.Request) -> web.Response:
    global _next_id
    data = await request.json()
    item = {"id": _next_id, "name": data.get("name", "")}
    _items[_next_id] = item
    _next_id += 1
    return web.json_response(item, status=201)


async def get_item(request: web.Request) -> web.Response:
    item = _items.get(int(request.match_info["item_id"]))
    if not item:
        return web.json_response({"error": "not found"}, status=404)
    return web.json_response(item)


app.add_routes([
    web.get("/items", list_items),
    web.post("/items", create_item),
    web.get("/items/{item_id}", get_item),
    web.static("/", "public"),
])


def main() -> None:
    web.run_app(app, host="127.0.0.1", port=8080)


if __name__ == "__main__":
    main()