import SwiftUI

/// The entry point for the SpriteKit game template.
/// Launches the app with a `GameView` containing the game scene.
@main
struct GameSpriteKitApp: App {
    var body: some Scene {
        WindowGroup {
            GameView()
        }
    }
}
