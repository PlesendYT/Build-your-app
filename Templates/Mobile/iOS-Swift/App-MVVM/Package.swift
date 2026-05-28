// swift-tools-version: 5.9
import PackageDescription

let package = Package(
    name: "AppMVVM",
    platforms: [.iOS(.v17)],
    products: [
        .library(name: "AppMVVM", targets: ["AppMVVM"])
    ],
    targets: [
        .target(name: "AppMVVM")
    ]
)
