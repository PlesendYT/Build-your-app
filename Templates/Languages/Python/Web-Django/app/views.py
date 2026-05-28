"""Views for the Django app."""

from __future__ import annotations

from django.http import HttpRequest, HttpResponse


def index(request: HttpRequest) -> HttpResponse:
    """Home page view."""
    name = request.GET.get("name", "World")
    return HttpResponse(f"<h1>Hello, {name}!</h1><p>Welcome to Django.</p>")
