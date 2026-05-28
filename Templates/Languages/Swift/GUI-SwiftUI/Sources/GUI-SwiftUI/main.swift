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

@Observable
final class AppModel {
    var items: [Item] = [
        Item(title: "Welcome", note: "Hello, SwiftUI!"),
        Item(title: "Getting Started", note: "Edit this file to begin."),
    ]
    var selectedItem: Item?

    func addItem() {
        let newItem = Item(title: "New Item \(items.count + 1)", note: "")
        items.append(newItem)
        selectedItem = newItem
    }

    func deleteItems(at offsets: IndexSet) {
        items.remove(atOffsets: offsets)
        selectedItem = nil
    }
}

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
