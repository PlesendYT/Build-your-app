"""Views for the items application."""

from django.views.generic import ListView, DetailView, CreateView
from django.urls import reverse_lazy
from .models import Item
from .forms import ContactForm
from django.shortcuts import render


class ItemListView(ListView):
    """List view showing all items."""

    model = Item
    template_name = 'home.html'
    context_object_name = 'items'


class ItemDetailView(DetailView):
    """Detail view for a single item."""

    model = Item
    template_name = 'item_detail.html'


def about(request):
    """Render the about page."""
    return render(request, 'about.html')


def contact(request):
    """Handle contact form display and submission."""
    form = ContactForm()
    if request.method == 'POST':
        form = ContactForm(request.POST)
        if form.is_valid():
            form.save()
            return render(request, 'contact.html', {'sent': True})
    return render(request, 'contact.html', {'form': form})
