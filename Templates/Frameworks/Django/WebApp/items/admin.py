"""Admin configuration for the items application."""

from django.contrib import admin
from .models import Item


@admin.register(Item)
class ItemAdmin(admin.ModelAdmin):
    """Admin interface for Item model."""

    list_display = ('title', 'created_at')
    search_fields = ('title',)
