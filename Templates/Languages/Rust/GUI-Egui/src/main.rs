use eframe::egui;

#[derive(Default, serde::Deserialize, serde::Serialize)]
struct EguiApp {
    name: String,
    counter: i32,
    items: Vec<String>,
}

impl eframe::App for EguiApp {
    fn save(&mut self, storage: &mut dyn eframe::Storage) {
        eframe::set_value(storage, "egui_app", self);
    }

    fn update(&mut self, ctx: &egui::Context, _frame: &mut eframe::Frame) {
        egui::CentralPanel::default().show(ctx, |ui| {
            ui.heading("Egui App");

            ui.horizontal(|ui| {
                ui.label("Name:");
                ui.text_edit_singleline(&mut self.name);
            });

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

            ui.horizontal(|ui| {
                let name = self.name.clone();
                if ui.button("Add Item").clicked() && !name.is_empty() {
                    self.items.push(name);
                    self.name.clear();
                }
            });

            ui.separator();
            egui::ScrollArea::vertical().show(ui, |ui| {
                for item in &self.items {
                    ui.label(item);
                }
            });
        });
    }
}

fn main() -> anyhow::Result<()> {
    let options = eframe::NativeOptions::default();
    eframe::run_native(
        "Egui Template",
        options,
        Box::new(|cc| Ok(Box::new(EguiApp::load_or_default(cc.storage)))),
    )?;
    Ok(())
}
