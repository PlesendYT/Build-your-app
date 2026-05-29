"""API views using Django REST Framework viewsets."""

from rest_framework import viewsets, permissions
from .serializers import ItemSerializer


class ItemViewSet(viewsets.ModelViewSet):
    """ViewSet for CRUD operations on Item resources."""

    queryset = []  # Replace with actual model's queryset
    serializer_class = ItemSerializer
    permission_classes = [permissions.IsAuthenticatedOrReadOnly]
