"""Item model for the Django WebApp."""

from django.db import models


class Item(models.Model):
    """Item with title, description, and auto-generated timestamp."""

    title = models.CharField(max_length=200)
    description = models.TextField()
    created_at = models.DateTimeField(auto_now_add=True)

    def __str__(self):
        return self.title
