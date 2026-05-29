"""API serializers for the Django REST API application."""

from rest_framework import serializers


class ItemSerializer(serializers.Serializer):
    """Serializer for Item model with read-only id and timestamps."""

    id = serializers.IntegerField(read_only=True)
    title = serializers.CharField(max_length=200)
    description = serializers.CharField()
    created_at = serializers.DateTimeField(read_only=True)
