// swift-tools-version: 5.9
import PackageDescription

let package = Package(
    name: "GameSceneKit",
    platforms: [.iOS(.v17)],
    products: [
        .library(name: "GameSceneKit", targets: ["GameSceneKit"])
    ],
    targets: [
        .target(name: "GameSceneKit")
    ]
)
