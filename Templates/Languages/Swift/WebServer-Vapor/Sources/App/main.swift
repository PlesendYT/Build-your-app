/// Vapor web server entry point with Leaf templating, middleware, and route configuration.
import Vapor
import Leaf

@main
enum Entry {
    /// Detects environment, bootstraps logging, and runs the application.
    static func main() async throws {
        var env = try Environment.detect()
        try LoggingSystem.bootstrap(from: &env)

        let app = Application(env)
        defer { app.shutdown() }

        // Serve static files from the Public directory
        app.middleware.use(FileMiddleware(publicDirectory: app.directory.publicDirectory))
        // Enable CORS for all origins
        app.middleware.use(CORSMiddleware(configuration: .init(
            allowedOrigin: .all,
            allowedMethods: [.GET, .POST, .PUT, .DELETE, .PATCH],
            allowedHeaders: [.accept, .authorization, .contentType]
        )))

        // Use Leaf for server-side templating
        app.views.use(.leaf)

        try configureRoutes(app)
        try app.run()
    }
}

/// Response model for the /health endpoint.
struct HealthResponse: Content {
    let status: String
    let version: String
    let uptime: Double
}

/// Registers all application routes including index, health, items, and hello endpoints.
func configureRoutes(_ app: Application) throws {
    // Render the index page with Leaf
    app.get { req async throws -> View in
        return try await req.view.render("index", ["title": "Vapor Server"])
    }

    // Health check returning status, version, and uptime
    app.get("health") { req -> HealthResponse in
        HealthResponse(
            status: "ok",
            version: "1.0.0",
            uptime: ProcessInfo.processInfo.systemUptime
        )
    }

    // List all sample items
    app.get("api", "items") { req async throws -> [Item] in
        return Item.sampleData
    }

    // Create a new item from request body
    app.post("api", "items") { req async throws -> Item in
        let item = try req.content.decode(Item.self)
        return item
    }

    // Personalized greeting using a route parameter
    app.get("hello", ":name") { req async throws -> String in
        let name = req.parameters.get("name") ?? "World"
        return "Hello, \(name)!"
    }
}

/// A sample item model conforming to Content and Identifiable.
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
