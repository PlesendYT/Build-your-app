from flask import Blueprint, render_template, request, redirect, url_for
from .models import Item

main = Blueprint('main', __name__)

@main.route('/')
def home():
    items = Item.query.all()
    return render_template('home.html', items=items)

@main.route('/about')
def about():
    return render_template('about.html')

@main.route('/contact', methods=['GET', 'POST'])
def contact():
    if request.method == 'POST':
        item = Item(
            title=f"Kontakt von {request.form['name']}",
            description=request.form['message']
        )
        item.save()
        return render_template('contact.html', sent=True)
    return render_template('contact.html')
