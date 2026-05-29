//! A minimal GTK4 application with a label, text entry, and click-counter button.

use gtk4::prelude::*;
use gtk4::{Application, ApplicationWindow, Button, Entry, Box as GtkBox, Label, Orientation};
use glib::clone;

/// Builds and runs the GTK4 application.
fn main() -> anyhow::Result<()> {
    let app = Application::builder()
        .application_id("com.example.gtk-template")
        .build();

    // Build the UI when the application is activated.
    app.connect_activate(|app| {
        let window = ApplicationWindow::builder()
            .application(app)
            .title("GTK4 Template")
            .default_width(400)
            .default_height(300)
            .build();

        // Vertical container for all widgets.
        let vbox = GtkBox::new(Orientation::Vertical, 8);
        vbox.set_margin_top(12);
        vbox.set_margin_bottom(12);
        vbox.set_margin_start(12);
        vbox.set_margin_end(12);

        // Title label.
        let label = Label::new(Some("Hello, GTK4!"));
        label.set_css_classes(&["title-1"]);
        vbox.append(&label);

        // Text entry with placeholder.
        let entry = Entry::new();
        entry.set_placeholder_text(Some("Enter text..."));
        vbox.append(&entry);

        // Label that displays the current counter value.
        let counter_label = Label::new(Some("Count: 0"));
        vbox.append(&counter_label);

        // Increment the counter each time the button is clicked.
        let count = std::cell::Cell::new(0i32);
        let button = Button::with_label("Click me");
        button.connect_clicked(clone!(@weak counter_label => move |_| {
            count.set(count.get() + 1);
            counter_label.set_text(&format!("Count: {}", count.get()));
        }));
        vbox.append(&button);

        window.set_child(Some(&vbox));
        window.present();
    });

    app.run_with_args(&[]);
    Ok(())
}
