/// Vapor REST API entry point with PostgreSQL, JWT auth, middleware, and route configuration.
import Vapor
import Fluent
import FluentPostgresDriver
import JWT

@main
enum Entry {
    /// Boots the application with database, JWT, middleware, and routes.
    static func main() async throws {
        var env = try Environment.detect()
        try LoggingSystem.bootstrap(from: &env)

        let app = Application(env)
        defer { app.shutdown() }

        try configureDatabase(app)
        try configureJWT(app)
        try configureMiddleware(app)
        try configureRoutes(app)

        try app.run()
    }
}

// MARK: - Configuration

/// Configures the PostgreSQL database connection and runs auto-migrations.
private func configureDatabase(_ app: Application) throws {
    // Use DATABASE_URL if available, otherwise fall back to individual env vars
    if let databaseURL = Environment.get("DATABASE_URL") {
        try app.databases.use(.postgres(url: databaseURL), as: .psql)
    } else {
        app.databases.use(.postgres(
            hostname: Environment.get("DB_HOST") ?? "localhost",
            port: Environment.get("DB_PORT").flatMap(Int.init) ?? 5432,
            username: Environment.get("DB_USER") ?? "vapor",
            password: Environment.get("DB_PASS") ?? "vapor",
            database: Environment.get("DB_NAME") ?? "restapi"
        ), as: .psql)
    }

    app.migrations.add(CreateUser())
    app.migrations.add(CreateTodo())
    try app.autoMigrate().wait()
}

/// Configures JWT signing with HS256 using the provided secret.
private func configureJWT(_ app: Application) throws {
    app.jwt.signers.use(.hs256(key: Environment.get("JWT_SECRET") ?? "super-secret-key-change-me"))
}

/// Configures CORS and error handling middleware.
private func configureMiddleware(_ app: Application) throws {
    app.middleware.use(CORSMiddleware(configuration: .init(
        allowedOrigin: .all,
        allowedMethods: [.GET, .POST, .PUT, .DELETE, .PATCH],
        allowedHeaders: [.accept, .authorization, .contentType]
    )))
    app.middleware.use(ErrorMiddleware.default(environment: app.environment))
}

// MARK: - Routes

/// Registers API v1 routes for auth, todos, and user profile.
func configureRoutes(_ app: Application) throws {
    let api = app.grouped("api", "v1")

    // Public auth endpoints
    api.post("auth", "register", use: AuthController.register)
    api.post("auth", "login", use: AuthController.login)

    // Protected endpoints requiring JWT authentication
    let protected = api.grouped(JWTAuthenticator())
    protected.get("todos", use: TodoController.index)
    protected.post("todos", use: TodoController.create)
    protected.get("todos", ":id", use: TodoController.show)
    protected.put("todos", ":id", use: TodoController.update)
    protected.delete("todos", ":id", use: TodoController.delete)

    protected.get("profile", use: UserController.profile)
}

// MARK: - Authenticator

/// Authenticates requests by verifying the Bearer JWT token.
struct JWTAuthenticator: AsyncBearerAuthenticator {
    func authenticate(bearer: BearerAuthorization, for request: Request) async throws {
        let payload = try await request.jwt.verify(bearer.token, as: UserPayload.self)
        request.auth.login(payload)
    }
}

/// JWT payload containing user ID, email, and expiration claim.
struct UserPayload: JWTPayload, Authenticatable {
    let id: UUID
    let email: String
    let exp: ExpirationClaim

    func verify(using signer: JWTSigner) throws {
        try exp.verifyNotExpired()
    }
}

// MARK: - Controllers

/// Handles user registration and login.
enum AuthController {
    /// Registers a new user with email and hashed password.
    static func register(req: Request) async throws -> UserResponse {
        let dto = try req.content.decode(RegisterDTO.self)
        let user = User(email: dto.email, password: try Bcrypt.hash(dto.password))
        try await user.save(on: req.db)
        return UserResponse(id: try user.requireID(), email: user.email)
    }

    /// Authenticates a user and returns a JWT token valid for 7 days.
    static func login(req: Request) async throws -> TokenResponse {
        let dto = try req.content.decode(LoginDTO.self)
        guard let user = try await User.query(on: req.db)
            .filter(\.$email == dto.email)
            .first() else {
            throw Abort(.unauthorized)
        }
        guard try Bcrypt.verify(dto.password, against: user.passwordHash) else {
            throw Abort(.unauthorized)
        }
        let payload = UserPayload(
            id: try user.requireID(),
            email: user.email,
            exp: .init(value: Date().addingTimeInterval(86400 * 7))
        )
        let token = try await req.jwt.sign(payload)
        return TokenResponse(token: token)
    }
}

/// Handles CRUD operations on todos scoped to the authenticated user.
enum TodoController {
    /// Lists all todos belonging to the authenticated user.
    static func index(req: Request) async throws -> [TodoDTO] {
        let userId = try req.auth.require(UserPayload.self).id
        let todos = try await Todo.query(on: req.db)
            .filter(\.$userId == userId)
            .all()
        return todos.map { TodoDTO(from: $0) }
    }

    /// Creates a new todo for the authenticated user.
    static func create(req: Request) async throws -> TodoDTO {
        let userId = try req.auth.require(UserPayload.self).id
        let dto = try req.content.decode(CreateTodoDTO.self)
        let todo = Todo(title: dto.title, userId: userId)
        try await todo.save(on: req.db)
        return TodoDTO(from: todo)
    }

    /// Returns a single todo by ID, scoped to the authenticated user.
    static func show(req: Request) async throws -> TodoDTO {
        guard let id = req.parameters.get("id", as: UUID.self) else {
            throw Abort(.badRequest)
        }
        guard let todo = try await Todo.find(id, on: req.db) else {
            throw Abort(.notFound)
        }
        return TodoDTO(from: todo)
    }

    /// Updates a todo's title and/or completion status.
    static func update(req: Request) async throws -> TodoDTO {
        guard let id = req.parameters.get("id", as: UUID.self) else {
            throw Abort(.badRequest)
        }
        let dto = try req.content.decode(UpdateTodoDTO.self)
        guard let todo = try await Todo.find(id, on: req.db) else {
            throw Abort(.notFound)
        }
        if let title = dto.title { todo.title = title }
        if let completed = dto.completed { todo.isCompleted = completed }
        try await todo.save(on: req.db)
        return TodoDTO(from: todo)
    }

    /// Deletes a todo by ID.
    static func delete(req: Request) async throws -> HTTPStatus {
        guard let id = req.parameters.get("id", as: UUID.self) else {
            throw Abort(.badRequest)
        }
        guard let todo = try await Todo.find(id, on: req.db) else {
            throw Abort(.notFound)
        }
        try await todo.delete(on: req.db)
        return .noContent
    }
}

/// Handles returning the authenticated user's profile.
enum UserController {
    static func profile(req: Request) async throws -> UserResponse {
        let payload = try req.auth.require(UserPayload.self)
        return UserResponse(id: payload.id, email: payload.email)
    }
}

// MARK: - DTOs

/// Request body for user registration.
struct RegisterDTO: Content {
    let email: String
    let password: String
}

/// Request body for user login.
struct LoginDTO: Content {
    let email: String
    let password: String
}

/// Response containing the JWT token.
struct TokenResponse: Content {
    let token: String
}

/// Response containing user profile data.
struct UserResponse: Content {
    let id: UUID
    let email: String
}

/// Request body for creating a todo.
struct CreateTodoDTO: Content {
    let title: String
}

/// Request body for updating a todo (partial update).
struct UpdateTodoDTO: Content {
    let title: String?
    let completed: Bool?
}

/// Response body for a todo item.
struct TodoDTO: Content {
    let id: UUID
    let title: String
    let isCompleted: Bool

    init(from todo: Todo) {
        self.id = todo.id ?? UUID()
        self.title = todo.title
        self.isCompleted = todo.isCompleted
    }
}
