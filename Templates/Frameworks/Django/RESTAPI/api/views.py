from rest_framework import viewsets, permissions
from .serializers import ItemSerializer

class ItemViewSet(viewsets.ModelViewSet):
    queryset = []  # Replace with actual model
    serializer_class = ItemSerializer
    permission_classes = [permissions.IsAuthenticatedOrReadOnly]
