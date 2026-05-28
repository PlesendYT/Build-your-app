// swift-tools-version: 5.9
import PackageDescription

let package = Package(
    name: "AppBasic",
    platforms: [.iOS(.v17)],
    products: [
        .library(name: "AppBasic", targets: ["AppBasic"])
    ],
    targets: [
        .target(name: "AppBasic")
    ]
)
