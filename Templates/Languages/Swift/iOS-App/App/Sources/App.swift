/// iOS task management app using SwiftUI and SwiftData for persistence.
import SwiftUI
import SwiftData

@main
struct iOSApp: App {
    let container: ModelContainer

    var body: some Scene {
        WindowGroup {
            ContentView()
                .modelContainer(container)
        }
    }

    /// Configures the SwiftData model container with the TaskItem schema.
    init() {
        do {
            let schema = Schema([TaskItem.self])
            let config = ModelConfiguration(isStoredInMemoryOnly: false)
            container = try ModelContainer(for: schema, configurations: config)
        } catch {
            fatalError("Failed to initialize ModelContainer: \(error)")
        }
    }
}

/// A persistent task model with title, note, completion status, and creation date.
@Model
final class TaskItem {
    @Attribute(.unique) var id: UUID
    var title: String
    var note: String
    var isCompleted: Bool
    var createdAt: Date

    init(title: String, note: String = "") {
        self.id = UUID()
        self.title = title
        self.note = note
        self.isCompleted = false
        self.createdAt = Date()
    }
}

/// Main list view showing all tasks with toggle, swipe-to-delete, and an add button.
struct ContentView: View {
    @Environment(\.modelContext) private var context
    @Query private var tasks: [TaskItem]
    @State private var showingAdd = false

    var body: some View {
        NavigationStack {
            List {
                ForEach(tasks) { task in
                    HStack {
                        // Toggle completion status
                        Button(action: { toggleTask(task) }) {
                            Image(systemName: task.isCompleted ? "checkmark.circle.fill" : "circle")
                        }
                        .buttonStyle(.plain)

                        VStack(alignment: .leading) {
                            Text(task.title)
                                .strikethrough(task.isCompleted)
                            Text(task.createdAt, style: .date)
                                .font(.caption)
                                .foregroundStyle(.secondary)
                        }
                    }
                }
                .onDelete(perform: deleteTasks)
            }
            .navigationTitle("Tasks")
            .toolbar {
                ToolbarItem(placement: .topBarTrailing) {
                    Button(action: { showingAdd = true }) {
                        Image(systemName: "plus")
                    }
                }
            }
            .sheet(isPresented: $showingAdd) {
                AddTaskView()
            }
        }
    }

    /// Toggles the completion state of a given task.
    private func toggleTask(_ task: TaskItem) {
        task.isCompleted.toggle()
    }

    /// Deletes tasks at the given index offsets from the list.
    private func deleteTasks(at offsets: IndexSet) {
        for index in offsets {
            context.delete(tasks[index])
        }
    }
}

/// Modal form view for creating a new task with a title and optional note.
struct AddTaskView: View {
    @Environment(\.modelContext) private var context
    @Environment(\.dismiss) private var dismiss
    @State private var title = ""
    @State private var note = ""

    var body: some View {
        NavigationStack {
            Form {
                TextField("Title", text: $title)
                TextField("Note", text: $note)
            }
            .navigationTitle("New Task")
            .navigationBarTitleDisplayMode(.inline)
            .toolbar {
                ToolbarItem(placement: .cancellationAction) {
                    Button("Cancel") { dismiss() }
                }
                ToolbarItem(placement: .confirmationAction) {
                    Button("Save") {
                        let task = TaskItem(title: title, note: note)
                        context.insert(task)
                        dismiss()
                    }
                    .disabled(title.isEmpty)
                }
            }
        }
    }
}
