from django import forms
from .models import Item

class ContactForm(forms.Form):
    name = forms.CharField(max_length=100)
    email = forms.EmailField()
    message = forms.CharField(widget=forms.Textarea)

    def save(self):
        Item.objects.create(
            title=f"Kontakt von {self.cleaned_data['name']}",
            description=self.cleaned_data['message']
        )
