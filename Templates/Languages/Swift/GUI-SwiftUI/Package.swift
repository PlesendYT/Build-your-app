// swift-tools-version: 5.9
/// Package definition for the GUI-SwiftUI executable.
/// A standalone macOS app built with SwiftUI.
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
