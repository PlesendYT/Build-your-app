// swift-tools-version: 5.9
import PackageDescription

let package = Package(
    name: "AppSwiftUI",
    platforms: [.iOS(.v17)],
    products: [
        .library(name: "AppSwiftUI", targets: ["AppSwiftUI"])
    ],
    targets: [
        .target(name: "AppSwiftUI")
    ]
)
