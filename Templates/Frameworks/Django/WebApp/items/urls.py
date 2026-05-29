"""URL patterns for the items application."""

from django.urls import path
from . import views

urlpatterns = [
    path('', views.ItemListView.as_view(), name='home'),
    path('item/<int:pk>/', views.ItemDetailView.as_view(), name='item_detail'),
    path('about/', views.about, name='about'),
    path('contact/', views.contact, name='contact'),
]
