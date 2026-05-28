// swift-tools-version: 5.9
import PackageDescription

let package = Package(
    name: "web-server",
    platforms: [.macOS(.v13)],
    dependencies: [
        .package(url: "https://github.com/vapor/vapor", from: "4.100.0"),
        .package(url: "https://github.com/vapor/leaf", from: "4.3.0"),
    ],
    targets: [
        .executableTarget(
            name: "App",
            dependencies: [
                .product(name: "Vapor", package: "vapor"),
                .product(name: "Leaf", package: "leaf"),
            ]
        ),
    ]
)
