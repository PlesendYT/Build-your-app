"""Root URL configuration for the Django WebApp project."""

from django.contrib import admin
from django.urls import path, include

urlpatterns = [
    path('admin/', admin.site.urls),
    path('', include('items.urls')),
]
