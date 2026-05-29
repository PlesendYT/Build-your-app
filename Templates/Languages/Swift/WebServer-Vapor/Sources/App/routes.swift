/// Vapor route registration and Todo controller with in-memory sample data.
import Vapor

/// Extension registering Todo CRUD routes under /api/v1.
extension Application {
    func registerRoutes() throws {
        let api = grouped("api", "v1")

        // Full CRUD for todos
        api.get("todos", use: TodoController.index)
        api.post("todos", use: TodoController.create)
        api.get("todos", ":id", use: TodoController.show)
        api.put("todos", ":id", use: TodoController.update)
        api.delete("todos", ":id", use: TodoController.delete)
    }
}

/// Handles Todo CRUD operations using in-memory sample data.
struct TodoController {
    /// Returns all sample todos.
    static func index(req: Request) async throws -> [Todo] {
        return Todo.sampleTodos
    }

    /// Creates a new todo from the decoded request body.
    static func create(req: Request) async throws -> Todo {
        let todo = try req.content.decode(Todo.self)
        return todo
    }

    /// Returns a single todo by ID, or 404 if not found.
    static func show(req: Request) async throws -> Todo {
        guard let id = req.parameters.get("id", as: Int.self) else {
            throw Abort(.badRequest, reason: "Invalid ID")
        }
        guard let todo = Todo.sampleTodos.first(where: { $0.id == id }) else {
            throw Abort(.notFound)
        }
        return todo
    }

    /// Replaces a todo with updated fields from the request body.
    static func update(req: Request) async throws -> Todo {
        guard let id = req.parameters.get("id", as: Int.self) else {
            throw Abort(.badRequest)
        }
        let updated = try req.content.decode(Todo.self)
        return updated
    }

    /// Deletes a todo by ID. Returns 204 No Content.
    static func delete(req: Request) async throws -> HTTPStatus {
        guard let _ = req.parameters.get("id", as: Int.self) else {
            throw Abort(.badRequest)
        }
        return .noContent
    }
}

/// A todo item model with id, title, and completion status.
struct Todo: Content, Identifiable {
    let id: Int
    var title: String
    var isCompleted: Bool

    static let sampleTodos: [Todo] = [
        Todo(id: 1, title: "Write code", isCompleted: false),
        Todo(id: 2, title: "Test API", isCompleted: false),
        Todo(id: 3, title: "Deploy", isCompleted: true),
    ]
}
