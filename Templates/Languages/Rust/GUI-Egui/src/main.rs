//! An egui-based desktop application with a name field, counter, and item list.
//! State is persisted to disk via serde.

use eframe::egui;

/// Top-level application state, serialized for persistence.
#[derive(Default, serde::Deserialize, serde::Serialize)]
struct EguiApp {
    name: String,
    counter: i32,
    items: Vec<String>,
}

impl eframe::App for EguiApp {
    /// Persist the app state to storage.
    fn save(&mut self, storage: &mut dyn eframe::Storage) {
        eframe::set_value(storage, "egui_app", self);
    }

    /// Build the UI each frame.
    fn update(&mut self, ctx: &egui::Context, _frame: &mut eframe::Frame) {
        egui::CentralPanel::default().show(ctx, |ui| {
            ui.heading("Egui App");

            // Name input field.
            ui.horizontal(|ui| {
                ui.label("Name:");
                ui.text_edit_singleline(&mut self.name);
            });

            // Increment / decrement counter.
            ui.horizontal(|ui| {
                if ui.button("+").clicked() {
                    self.counter += 1;
                }
                ui.label(format!("Counter: {}", self.counter));
                if ui.button("-").clicked() {
                    self.counter -= 1;
                }
            });

            ui.separator();

            // Add the current name as an item.
            ui.horizontal(|ui| {
                let name = self.name.clone();
                if ui.button("Add Item").clicked() && !name.is_empty() {
                    self.items.push(name);
                    self.name.clear();
                }
            });

            ui.separator();

            // Scrollable list of added items.
            egui::ScrollArea::vertical().show(ui, |ui| {
                for item in &self.items {
                    ui.label(item);
                }
            });
        });
    }
}

/// Entry point: creates the native window and runs the egui app.
fn main() -> anyhow::Result<()> {
    let options = eframe::NativeOptions::default();
    eframe::run_native(
        "Egui Template",
        options,
        Box::new(|cc| Ok(Box::new(EguiApp::load_or_default(cc.storage)))),
    )?;
    Ok(())
}
