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

struct GameView: View {
    @State private var score = 0
    @State private var gameState: GameState = .menu

    var scene: SKScene {
        let scene = GameScene(size: CGSize(width: 390, height: 844))
        scene.scaleMode = .aspectFill
        scene.onScoreUpdate = { score = $0 }
        scene.onGameOver = { gameState = .gameOver }
        return scene
    }

    var body: some View {
        ZStack {
            SpriteView(scene: scene)
                .ignoresSafeArea()

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

enum GameState {
    case menu, playing, gameOver
}

final class GameScene: SKScene, SKPhysicsContactDelegate {
    var onScoreUpdate: ((Int) -> Void)?
    var onGameOver: (() -> Void)?

    private let playerCategory: UInt32 = 0x1 << 0
    private let obstacleCategory: UInt32 = 0x1 << 1

    private var player: SKNode!
    private var score = 0
    private var spawnInterval: TimeInterval = 1.5

    override func didMove(to view: SKView) {
        backgroundColor = .black
        physicsWorld.gravity = .zero
        physicsWorld.contactDelegate = self

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

    override func touchesMoved(_ touches: Set<UITouch>, with event: UIEvent?) {
        guard let touch = touches.first else { return }
        let location = touch.location(in: self)
        player.position.x = location.x.clamped(to: 20...frame.width - 20)
    }

    override func update(_ currentTime: TimeInterval) {
        spawnInterval -= 0.01
        if spawnInterval <= 0 {
            spawnObstacle()
            spawnInterval = 1.5
        }
    }

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

        let moveAction = SKAction.moveBy(x: 0, y: -frame.height - 60, duration: 3)
        let removeAction = SKAction.removeFromParent()
        obstacle.run(SKAction.sequence([moveAction, removeAction]))
    }

    func didBegin(_ contact: SKPhysicsContact) {
        onGameOver?()
        isPaused = true
    }
}

extension CGFloat {
    func clamped(to range: ClosedRange<CGFloat>) -> CGFloat {
        return min(max(self, range.lowerBound), range.upperBound)
    }
}
