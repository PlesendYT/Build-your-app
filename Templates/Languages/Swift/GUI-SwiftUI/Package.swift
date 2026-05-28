// swift-tools-version: 5.9
import PackageDescription

let package = Package(
    name: "gui-swiftui",
    platforms: [.macOS(.v14)],
    targets: [
        .executableTarget(
            name: "GUI-SwiftUI"
        ),
    ]
)
