import SwiftUI

/// The entry point for the SceneKit game template.
/// Launches the app with a 3D `SceneKitView`.
@main
struct GameSceneKitApp: App {
    var body: some Scene {
        WindowGroup {
            SceneKitView()
        }
    }
}
