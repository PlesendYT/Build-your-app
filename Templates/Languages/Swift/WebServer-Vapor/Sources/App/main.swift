import Vapor
import Leaf

@main
enum Entry {
    static func main() async throws {
        var env = try Environment.detect()
        try LoggingSystem.bootstrap(from: &env)

        let app = Application(env)
        defer { app.shutdown() }

        app.middleware.use(FileMiddleware(publicDirectory: app.directory.publicDirectory))
        app.middleware.use(CORSMiddleware(configuration: .init(
            allowedOrigin: .all,
            allowedMethods: [.GET, .POST, .PUT, .DELETE, .PATCH],
            allowedHeaders: [.accept, .authorization, .contentType]
        )))

        app.views.use(.leaf)

        try configureRoutes(app)
        try app.run()
    }
}

struct HealthResponse: Content {
    let status: String
    let version: String
    let uptime: Double
}

func configureRoutes(_ app: Application) throws {
    app.get { req async throws -> View in
        return try await req.view.render("index", ["title": "Vapor Server"])
    }

    app.get("health") { req -> HealthResponse in
        HealthResponse(
            status: "ok",
            version: "1.0.0",
            uptime: ProcessInfo.processInfo.systemUptime
        )
    }

    app.get("api", "items") { req async throws -> [Item] in
        return Item.sampleData
    }

    app.post("api", "items") { req async throws -> Item in
        let item = try req.content.decode(Item.self)
        return item
    }

    app.get("hello", ":name") { req async throws -> String in
        let name = req.parameters.get("name") ?? "World"
        return "Hello, \(name)!"
    }
}

struct Item: Content, Identifiable {
    let id: Int
    let title: String
    let completed: Bool

    static let sampleData: [Item] = [
        Item(id: 1, title: "Learn Vapor", completed: false),
        Item(id: 2, title: "Build an API", completed: false),
        Item(id: 3, title: "Deploy to production", completed: true),
    ]
}
