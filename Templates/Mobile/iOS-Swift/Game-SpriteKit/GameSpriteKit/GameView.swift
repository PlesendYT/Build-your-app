import SwiftUI
import SpriteKit

/// A SwiftUI view that wraps a SpriteKit `GameScene` and fills the entire screen.
struct GameView: View {
    /// Creates and configures the `GameScene` with the current screen size.
    var scene: SKScene {
        let scene = GameScene(size: UIScreen.main.bounds.size)
        scene.scaleMode = .resizeFill
        return scene
    }

    var body: some View {
        // Embed the SpriteKit scene and ignore safe areas for full-screen gameplay.
        SpriteView(scene: scene)
            .ignoresSafeArea()
    }
}

#Preview {
    GameView()
}
