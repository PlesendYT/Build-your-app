// swift-tools-version: 5.9
import PackageDescription

let package = Package(
    name: "rest-api",
    platforms: [.macOS(.v13)],
    dependencies: [
        .package(url: "https://github.com/vapor/vapor", from: "4.100.0"),
        .package(url: "https://github.com/vapor/fluent", from: "4.10.0"),
        .package(url: "https://github.com/vapor/fluent-postgres-driver", from: "2.8.0"),
        .package(url: "https://github.com/vapor/jwt", from: "4.3.0"),
    ],
    targets: [
        .executableTarget(
            name: "App",
            dependencies: [
                .product(name: "Vapor", package: "vapor"),
                .product(name: "Fluent", package: "fluent"),
                .product(name: "FluentPostgresDriver", package: "fluent-postgres-driver"),
                .product(name: "JWT", package: "jwt"),
            ]
        ),
    ]
)
