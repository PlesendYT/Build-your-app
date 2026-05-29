// swift-tools-version: 5.9
/// Package definition for the GUI-AppKit executable.
/// A standalone macOS app using the AppKit framework.
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
