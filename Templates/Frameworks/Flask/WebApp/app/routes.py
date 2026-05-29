"""Route definitions for the Flask WebApp."""

from flask import Blueprint, render_template, request, redirect, url_for
from .models import Item

main = Blueprint('main', __name__)


@main.route('/')
def home():
    """Render the home page with all items."""
    items = Item.query.all()
    return render_template('home.html', items=items)


@main.route('/about')
def about():
    """Render the about page."""
    return render_template('about.html')


@main.route('/contact', methods=['GET', 'POST'])
def contact():
    """Handle contact form display and submission."""
    if request.method == 'POST':
        item = Item(
            title=f"Kontakt von {request.form['name']}",
            description=request.form['message']
        )
        item.save()
        return render_template('contact.html', sent=True)
    return render_template('contact.html')
