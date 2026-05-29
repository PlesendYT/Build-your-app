// swift-tools-version: 5.9
/// Package definition for the CLI-Tool executable.
/// Depends on swift-argument-parser for command-line argument handling.
import PackageDescription

let package = Package(
    name: "cli-tool",
    platforms: [.macOS(.v13)],
    dependencies: [
        .package(url: "https://github.com/apple/swift-argument-parser", from: "1.3.0"),
    ],
    targets: [
        .executableTarget(
            name: "CLI-Tool",
            dependencies: [
                .product(name: "ArgumentParser", package: "swift-argument-parser"),
            ]
        ),
    ]
)
