"""Form definitions for the items application."""

from django import forms
from .models import Item


class ContactForm(forms.Form):
    """Contact form with name, email, and message fields."""

    name = forms.CharField(max_length=100)
    email = forms.EmailField()
    message = forms.CharField(widget=forms.Textarea)

    def save(self):
        """Create an Item from the submitted contact form data."""
        Item.objects.create(
            title=f"Kontakt von {self.cleaned_data['name']}",
            description=self.cleaned_data['message']
        )
