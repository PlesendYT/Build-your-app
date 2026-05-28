// swift-tools-version: 5.9
import PackageDescription

let package = Package(
    name: "gui-appkit",
    platforms: [.macOS(.v13)],
    targets: [
        .executableTarget(
            name: "GUI-AppKit"
        ),
    ]
)
