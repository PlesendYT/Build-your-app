import Vapor

extension Application {
    func registerRoutes() throws {
        let api = grouped("api", "v1")

        api.get("todos", use: TodoController.index)
        api.post("todos", use: TodoController.create)
        api.get("todos", ":id", use: TodoController.show)
        api.put("todos", ":id", use: TodoController.update)
        api.delete("todos", ":id", use: TodoController.delete)
    }
}

struct TodoController {
    static func index(req: Request) async throws -> [Todo] {
        return Todo.sampleTodos
    }

    static func create(req: Request) async throws -> Todo {
        let todo = try req.content.decode(Todo.self)
        return todo
    }

    static func show(req: Request) async throws -> Todo {
        guard let id = req.parameters.get("id", as: Int.self) else {
            throw Abort(.badRequest, reason: "Invalid ID")
        }
        guard let todo = Todo.sampleTodos.first(where: { $0.id == id }) else {
            throw Abort(.notFound)
        }
        return todo
    }

    static func update(req: Request) async throws -> Todo {
        guard let id = req.parameters.get("id", as: Int.self) else {
            throw Abort(.badRequest)
        }
        let updated = try req.content.decode(Todo.self)
        return updated
    }

    static func delete(req: Request) async throws -> HTTPStatus {
        guard let _ = req.parameters.get("id", as: Int.self) else {
            throw Abort(.badRequest)
        }
        return .noContent
    }
}

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
