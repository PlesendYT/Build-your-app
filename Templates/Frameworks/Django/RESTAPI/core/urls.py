"""Root URL configuration for the Django REST API project."""

from django.contrib import admin
from django.urls import path, include
from rest_framework.routers import DefaultRouter
from api.views import ItemViewSet

# REST framework router for the Item endpoint
router = DefaultRouter()
router.register('items', ItemViewSet)

urlpatterns = [
    path('admin/', admin.site.urls),
    path('api/', include(router.urls)),
    path('api-auth/', include('rest_framework.urls')),
]
