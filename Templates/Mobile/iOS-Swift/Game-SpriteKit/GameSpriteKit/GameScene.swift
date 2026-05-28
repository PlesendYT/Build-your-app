import SpriteKit
import GameplayKit

class GameScene: SKScene, SKPhysicsContactDelegate {
    private var player: SKSpriteNode!
    private var scoreLabel: SKLabelNode!
    private var score = 0

    private let playerCategory: UInt32 = 0x1 << 0
    private let coinCategory: UInt32 = 0x1 << 1
    private let obstacleCategory: UInt32 = 0x1 << 2

    override func didMove(to view: SKView) {
        backgroundColor = .systemBackground
        physicsWorld.contactDelegate = self
        physicsWorld.gravity = .zero

        player = SKSpriteNode(color: .systemBlue, size: CGSize(width: 40, height: 40))
        player.position = CGPoint(x: frame.midX, y: 100)
        player.physicsBody = SKPhysicsBody(rectangleOf: player.size)
        player.physicsBody?.categoryBitMask = playerCategory
        player.physicsBody?.contactTestBitMask = coinCategory | obstacleCategory
        player.physicsBody?.isDynamic = true
        addChild(player)

        scoreLabel = SKLabelNode(text: "Score: 0")
        scoreLabel.position = CGPoint(x: frame.midX, y: frame.maxY - 80)
        scoreLabel.fontSize = 24
        addChild(scoreLabel)

        run(SKAction.repeatForever(SKAction.sequence([
            SKAction.run(spawnCoin),
            SKAction.wait(forDuration: 1.5)
        ])))

        run(SKAction.repeatForever(SKAction.sequence([
            SKAction.run(spawnObstacle),
            SKAction.wait(forDuration: 2.0)
        ])))
    }

    func spawnCoin() {
        let coin = SKSpriteNode(color: .systemYellow, size: CGSize(width: 20, height: 20))
        coin.position = CGPoint(x: CGFloat.random(in: 20...frame.maxX - 20), y: frame.maxY + 20)
        coin.physicsBody = SKPhysicsBody(rectangleOf: coin.size)
        coin.physicsBody?.categoryBitMask = coinCategory
        coin.physicsBody?.contactTestBitMask = playerCategory
        coin.physicsBody?.isDynamic = true
        addChild(coin)
        coin.run(SKAction.moveTo(y: -20, duration: 3))
    }

    func spawnObstacle() {
        let obstacle = SKSpriteNode(color: .systemRed, size: CGSize(width: 30, height: 30))
        obstacle.position = CGPoint(x: CGFloat.random(in: 20...frame.maxX - 20), y: frame.maxY + 20)
        obstacle.physicsBody = SKPhysicsBody(rectangleOf: obstacle.size)
        obstacle.physicsBody?.categoryBitMask = obstacleCategory
        obstacle.physicsBody?.contactTestBitMask = playerCategory
        obstacle.physicsBody?.isDynamic = true
        addChild(obstacle)
        obstacle.run(SKAction.moveTo(y: -20, duration: 2.5))
    }

    override func touchesMoved(_ touches: Set<UITouch>, with event: UIEvent?) {
        guard let touch = touches.first else { return }
        let location = touch.location(in: self)
        player.position.x = location.x
    }

    func didBegin(_ contact: SKPhysicsContact) {
        let bodyA = contact.bodyA
        let bodyB = contact.bodyB

        if bodyA.categoryBitMask == coinCategory || bodyB.categoryBitMask == coinCategory {
            let coin = bodyA.categoryBitMask == coinCategory ? bodyA.node : bodyB.node
            coin?.removeFromParent()
            score += 1
            scoreLabel.text = "Score: \(score)"
        }

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
