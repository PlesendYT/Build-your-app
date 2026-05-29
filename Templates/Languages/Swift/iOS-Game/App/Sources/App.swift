/// iOS space-dodge game built with SwiftUI and SpriteKit.
import SwiftUI
import SpriteKit

@main
struct iOSGame: App {
    var body: some Scene {
        WindowGroup {
            GameView()
                .ignoresSafeArea()
        }
    }
}

/// Manages the game UI overlay, score display, and menu/game-over screens.
struct GameView: View {
    @State private var score = 0
    @State private var gameState: GameState = .menu

    /// Configures and returns the SpriteKit scene with callbacks for score and game-over.
    var scene: SKScene {
        let scene = GameScene(size: CGSize(width: 390, height: 844))
        scene.scaleMode = .aspectFill
        scene.onScoreUpdate = { score = $0 }
        scene.onGameOver = { gameState = .gameOver }
        return scene
    }

    var body: some View {
        ZStack {
            // SpriteKit rendering layer
            SpriteView(scene: scene)
                .ignoresSafeArea()

            // Score display at top
            VStack {
                HStack {
                    Text("Score: \(score)")
                        .font(.title2.monospacedDigit())
                        .foregroundStyle(.white)
                        .padding()
                    Spacer()
                }
                Spacer()
            }

            // Main menu overlay
            if gameState == .menu {
                VStack(spacing: 20) {
                    Text("Space Dodge")
                        .font(.largeTitle.bold())
                        .foregroundStyle(.white)
                    Button("Start") {
                        gameState = .playing
                        scene.isPaused = false
                    }
                    .buttonStyle(.borderedProminent)
                }
            }

            // Game-over overlay with final score
            if gameState == .gameOver {
                VStack(spacing: 20) {
                    Text("Game Over")
                        .font(.largeTitle.bold())
                        .foregroundStyle(.white)
                    Text("Score: \(score)")
                        .font(.title)
                        .foregroundStyle(.white)
                    Button("Play Again") {
                        gameState = .menu
                        score = 0
                        scene.isPaused = true
                    }
                    .buttonStyle(.borderedProminent)
                }
            }
        }
    }
}

/// Possible states of the game: menu, actively playing, or game-over.
enum GameState {
    case menu, playing, gameOver
}

/// SpriteKit scene handling player movement, obstacle spawning, and collision detection.
final class GameScene: SKScene, SKPhysicsContactDelegate {
    var onScoreUpdate: ((Int) -> Void)?
    var onGameOver: (() -> Void)?

    private let playerCategory: UInt32 = 0x1 << 0
    private let obstacleCategory: UInt32 = 0x1 << 1

    private var player: SKNode!
    private var score = 0
    private var spawnInterval: TimeInterval = 1.5

    /// Sets up the player node and physics world when the scene is presented.
    override func didMove(to view: SKView) {
        backgroundColor = .black
        physicsWorld.gravity = .zero
        physicsWorld.contactDelegate = self

        // Create player rectangle with physics body
        let playerSize = CGSize(width: 40, height: 40)
        player = SKShapeNode(rectOf: playerSize, cornerRadius: 8)
        player.fillColor = .cyan
        player.strokeColor = .white
        player.position = CGPoint(x: frame.midX, y: 100)
        player.physicsBody = SKPhysicsBody(rectangleOf: playerSize)
        player.physicsBody?.categoryBitMask = playerCategory
        player.physicsBody?.contactTestBitMask = obstacleCategory
        player.physicsBody?.isDynamic = true
        addChild(player)
    }

    /// Moves the player horizontally to follow the touch location.
    override func touchesMoved(_ touches: Set<UITouch>, with event: UIEvent?) {
        guard let touch = touches.first else { return }
        let location = touch.location(in: self)
        player.position.x = location.x.clamped(to: 20...frame.width - 20)
    }

    /// Gradually decreases spawn interval and triggers obstacle creation.
    override func update(_ currentTime: TimeInterval) {
        spawnInterval -= 0.01
        if spawnInterval <= 0 {
            spawnObstacle()
            spawnInterval = 1.5
        }
    }

    /// Creates a red obstacle at a random x position and animates it downward.
    private func spawnObstacle() {
        let size = CGSize(width: 30, height: 30)
        let obstacle = SKShapeNode(rectOf: size, cornerRadius: 4)
        obstacle.fillColor = .red
        obstacle.position = CGPoint(
            x: CGFloat.random(in: 20...frame.width - 20),
            y: frame.height + 30
        )
        obstacle.physicsBody = SKPhysicsBody(rectangleOf: size)
        obstacle.physicsBody?.categoryBitMask = obstacleCategory
        obstacle.physicsBody?.isDynamic = false
        addChild(obstacle)

        // Move obstacle down and remove it when off-screen
        let moveAction = SKAction.moveBy(x: 0, y: -frame.height - 60, duration: 3)
        let removeAction = SKAction.removeFromParent()
        obstacle.run(SKAction.sequence([moveAction, removeAction]))
    }

    /// Called when the player collides with an obstacle; triggers game-over.
    func didBegin(_ contact: SKPhysicsContact) {
        onGameOver?()
        isPaused = true
    }
}

/// Utility to clamp a CGFloat within a closed range.
extension CGFloat {
    func clamped(to range: ClosedRange<CGFloat>) -> CGFloat {
        return min(max(self, range.lowerBound), range.upperBound)
    }
}
