// swift-tools-version: 5.9
import PackageDescription

let package = Package(
    name: "GameSpriteKit",
    platforms: [.iOS(.v17)],
    products: [
        .library(name: "GameSpriteKit", targets: ["GameSpriteKit"])
    ],
    targets: [
        .target(name: "GameSpriteKit")
    ]
)
