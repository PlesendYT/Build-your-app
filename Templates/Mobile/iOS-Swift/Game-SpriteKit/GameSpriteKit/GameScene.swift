import SpriteKit
import GameplayKit

/// The main game scene where the player collects coins and avoids obstacles.
/// Uses SpriteKit physics for collision detection.
class GameScene: SKScene, SKPhysicsContactDelegate {
    /// The player-controlled blue square.
    private var player: SKSpriteNode!
    /// The label that displays the current score.
    private var scoreLabel: SKLabelNode!
    /// The player's current score.
    private var score = 0

    /// Bitmask categories for physics collisions.
    private let playerCategory: UInt32 = 0x1 << 0
    private let coinCategory: UInt32 = 0x1 << 1
    private let obstacleCategory: UInt32 = 0x1 << 2

    /// Called when the scene is presented by a view. Sets up the player, score label,
    /// and repeating timers for spawning coins and obstacles.
    override func didMove(to view: SKView) {
        backgroundColor = .systemBackground
        physicsWorld.contactDelegate = self
        physicsWorld.gravity = .zero

        // Create and position the player at the bottom center.
        player = SKSpriteNode(color: .systemBlue, size: CGSize(width: 40, height: 40))
        player.position = CGPoint(x: frame.midX, y: 100)
        player.physicsBody = SKPhysicsBody(rectangleOf: player.size)
        player.physicsBody?.categoryBitMask = playerCategory
        player.physicsBody?.contactTestBitMask = coinCategory | obstacleCategory
        player.physicsBody?.isDynamic = true
        addChild(player)

        // Score label anchored near the top of the screen.
        scoreLabel = SKLabelNode(text: "Score: 0")
        scoreLabel.position = CGPoint(x: frame.midX, y: frame.maxY - 80)
        scoreLabel.fontSize = 24
        addChild(scoreLabel)

        // Spawn a coin every 1.5 seconds.
        run(SKAction.repeatForever(SKAction.sequence([
            SKAction.run(spawnCoin),
            SKAction.wait(forDuration: 1.5)
        ])))

        // Spawn an obstacle every 2.0 seconds.
        run(SKAction.repeatForever(SKAction.sequence([
            SKAction.run(spawnObstacle),
            SKAction.wait(forDuration: 2.0)
        ])))
    }

    /// Spawns a yellow coin at a random x position at the top of the screen
    /// and animates it falling downward.
    func spawnCoin() {
        let coin = SKSpriteNode(color: .systemYellow, size: CGSize(width: 20, height: 20))
        coin.position = CGPoint(x: CGFloat.random(in: 20...frame.maxX - 20), y: frame.maxY + 20)
        coin.physicsBody = SKPhysicsBody(rectangleOf: coin.size)
        coin.physicsBody?.categoryBitMask = coinCategory
        coin.physicsBody?.contactTestBitMask = playerCategory
        coin.physicsBody?.isDynamic = true
        addChild(coin)
        // Move the coin from top to bottom over 3 seconds.
        coin.run(SKAction.moveTo(y: -20, duration: 3))
    }

    /// Spawns a red obstacle at a random x position at the top of the screen
    /// and animates it falling downward.
    func spawnObstacle() {
        let obstacle = SKSpriteNode(color: .systemRed, size: CGSize(width: 30, height: 30))
        obstacle.position = CGPoint(x: CGFloat.random(in: 20...frame.maxX - 20), y: frame.maxY + 20)
        obstacle.physicsBody = SKPhysicsBody(rectangleOf: obstacle.size)
        obstacle.physicsBody?.categoryBitMask = obstacleCategory
        obstacle.physicsBody?.contactTestBitMask = playerCategory
        obstacle.physicsBody?.isDynamic = true
        addChild(obstacle)
        // Move the obstacle from top to bottom over 2.5 seconds.
        obstacle.run(SKAction.moveTo(y: -20, duration: 2.5))
    }

    /// Moves the player horizontally to follow the user's touch position.
    override func touchesMoved(_ touches: Set<UITouch>, with event: UIEvent?) {
        guard let touch = touches.first else { return }
        let location = touch.location(in: self)
        player.position.x = location.x
    }

    /// Handles collision detection between the player, coins, and obstacles.
    func didBegin(_ contact: SKPhysicsContact) {
        let bodyA = contact.bodyA
        let bodyB = contact.bodyB

        // If the player touches a coin, remove the coin and increment the score.
        if bodyA.categoryBitMask == coinCategory || bodyB.categoryBitMask == coinCategory {
            let coin = bodyA.categoryBitMask == coinCategory ? bodyA.node : bodyB.node
            coin?.removeFromParent()
            score += 1
            scoreLabel.text = "Score: \(score)"
        }

        // If the player touches an obstacle, end the game.
        if bodyA.categoryBitMask == obstacleCategory || bodyB.categoryBitMask == obstacleCategory {
            player.removeFromParent()
            let gameOver = SKLabelNode(text: "Game Over")
            gameOver.fontSize = 48
            gameOver.position = CGPoint(x: frame.midX, y: frame.midY)
            addChild(gameOver)
            isPaused = true
        }
    }
}
