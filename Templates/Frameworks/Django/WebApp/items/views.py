from django.views.generic import ListView, DetailView, CreateView
from django.urls import reverse_lazy
from .models import Item
from .forms import ContactForm
from django.shortcuts import render

class ItemListView(ListView):
    model = Item
    template_name = 'home.html'
    context_object_name = 'items'

class ItemDetailView(DetailView):
    model = Item
    template_name = 'item_detail.html'

def about(request):
    return render(request, 'about.html')

def contact(request):
    form = ContactForm()
    if request.method == 'POST':
        form = ContactForm(request.POST)
        if form.is_valid():
            form.save()
            return render(request, 'contact.html', {'sent': True})
    return render(request, 'contact.html', {'form': form})
