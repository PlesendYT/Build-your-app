/// SwiftUI-based macOS application with a navigation split view and item management.
import SwiftUI

@main
struct SwiftUIApp: App {
    @State private var model = AppModel()

    var body: some Scene {
        WindowGroup {
            ContentView()
                .environment(model)
        }
        .windowResizability(.contentMinSize)
    }
}

/// Observable model holding the list of items and the currently selected item.
@Observable
final class AppModel {
    var items: [Item] = [
        Item(title: "Welcome", note: "Hello, SwiftUI!"),
        Item(title: "Getting Started", note: "Edit this file to begin."),
    ]
    var selectedItem: Item?

    /// Creates a new item, appends it to the list, and selects it.
    func addItem() {
        let newItem = Item(title: "New Item \(items.count + 1)", note: "")
        items.append(newItem)
        selectedItem = newItem
    }

    /// Removes items at the specified offsets and clears selection.
    func deleteItems(at offsets: IndexSet) {
        items.remove(atOffsets: offsets)
        selectedItem = nil
    }
}

/// A single item with a title and note, identifiable and codable.
struct Item: Identifiable, Codable, Hashable {
    let id: UUID
    var title: String
    var note: String

    init(id: UUID = UUID(), title: String, note: String) {
        self.id = id
        self.title = title
        self.note = note
    }
}

/// Main view with a sidebar list and a detail panel for the selected item.
struct ContentView: View {
    @Environment(AppModel.self) private var model

    var body: some View {
        NavigationSplitView {
            List(selection: Bindable(model).selectedItem) {
                ForEach(model.items) { item in
                    NavigationLink(value: item) {
                        Text(item.title)
                            .font(.headline)
                    }
                }
                .onDelete(perform: model.deleteItems)
            }
            .navigationTitle("Items")
            .toolbar {
                ToolbarItem(placement: .primaryAction) {
                    Button(action: model.addItem) {
                        Label("Add", systemImage: "plus")
                    }
                }
            }
        } detail: {
            // Show item detail or placeholder when nothing is selected
            if let selectedItem = model.selectedItem {
                ItemDetailView(item: selectedItem)
            } else {
                ContentUnavailableView(
                    "No Selection",
                    systemImage: "sidebar.left",
                    description: Text("Select an item from the list.")
                )
            }
        }
    }
}

/// Form view for editing a single item's title and note.
struct ItemDetailView: View {
    @Bindable var item: Item

    var body: some View {
        Form {
            TextField("Title", text: $item.title)
            TextEditor(text: $item.note)
                .frame(minHeight: 200)
        }
        .padding()
        .navigationTitle(item.title)
    }
}
