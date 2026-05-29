import raylib, math, sequtils, random, strformat

## Game window dimensions and gameplay constants
const
  ScreenWidth = 800
  ScreenHeight = 600
  PlayerSpeed = 5.0
  MaxBullets = 32
  MaxEnemies = 15

## Player ship with position, size, and color
type
  Player = object
    x, y: float32
    radius: float32
    color: Color

## Projectile fired by the player
  Bullet = object
    x, y: float32
    active: bool

## Hostile enemy that moves downward
  Enemy = object
    x, y: float32
    radius: float32
    speed: float32
    active: bool

## Possible game states: menu, playing, or game over
  GameState = enum
    gsMenu, gsPlaying, gsGameOver

## Creates a new player at the bottom center of the screen
proc initPlayer(): Player =
  Player(
    x: ScreenWidth.float32 / 2,
    y: ScreenHeight.float32 - 50,
    radius: 20,
    color: Blue
  )

## Moves the player based on keyboard input, clamped to screen bounds
proc updatePlayer(player: var Player) =
  if isKeyDown(KEY_LEFT) or isKeyDown(KEY_A):
    player.x -= PlayerSpeed
  if isKeyDown(KEY_RIGHT) or isKeyDown(KEY_D):
    player.x += PlayerSpeed
  if isKeyDown(KEY_UP) or isKeyDown(KEY_W):
    player.y -= PlayerSpeed
  if isKeyDown(KEY_DOWN) or isKeyDown(KEY_S):
    player.y += PlayerSpeed

  player.x = clamp(player.x, player.radius, ScreenWidth.float32 - player.radius)
  player.y = clamp(player.y, player.radius, ScreenHeight.float32 - player.radius)

## Renders the player ship as a filled circle
proc drawPlayer(player: Player) =
  drawCircle(int32(player.x), int32(player.y), player.radius, player.color)

## Creates a new active bullet at the given position
proc initBullet(x, y: float32): Bullet =
  Bullet(x: x, y: y, active: true)

## Moves the bullet upward; deactivates when it leaves the screen
proc updateBullet(bullet: var Bullet) =
  if bullet.active:
    bullet.y -= 7
    if bullet.y < 0:
      bullet.active = false

## Draws the bullet as a small yellow circle if active
proc drawBullet(bullet: Bullet) =
  if bullet.active:
    drawCircle(int32(bullet.x), int32(bullet.y), 5, Yellow)

## Creates a new active enemy at the given position
proc initEnemy(x, y: float32): Enemy =
  Enemy(x: x, y: y, radius: 15, speed: 2.0, active: true)

## Moves the enemy downward; deactivates when it leaves the screen
proc updateEnemy(enemy: var Enemy) =
  if enemy.active:
    enemy.y += enemy.speed
    if enemy.y > ScreenHeight.float32 + enemy.radius:
      enemy.active = false

## Draws the enemy as a red circle if active
proc drawEnemy(enemy: Enemy) =
  if enemy.active:
    drawCircle(int32(enemy.x), int32(enemy.y), enemy.radius, Red)

## Circle-circle collision detection using distance comparison
proc checkCollision(aX, aY, aR, bX, bY, bR: float32): bool =
  let dx = aX - bX
  let dy = aY - bY
  sqrt(dx * dx + dy * dy) < aR + bR

## Main game loop handling menu, gameplay, and game-over states
proc gameLoop() =
  randomize()
  initWindow(ScreenWidth, ScreenHeight, "Nim Game")
  defer closeWindow()

  setTargetFPS(60)

  var player = initPlayer()
  var bullets: array[MaxBullets, Bullet]
  var enemies: array[MaxEnemies, Enemy]
  var enemyCount = 0
  var score = 0
  var spawnTimer = 0.0
  var state = gsMenu

  while not windowShouldClose():
    # Handle current game state logic
    case state
    of gsMenu:
      # Reset game when SPACE is pressed
      if isKeyPressed(KEY_SPACE):
        state = gsPlaying
        player = initPlayer()
        score = 0
        enemyCount = 0
        for e in enemies.mitems:
          e.active = false

    of gsPlaying:
      player.updatePlayer()

      # Fire a new bullet on SPACE if any slot is available
      if isKeyPressed(KEY_SPACE):
        for b in bullets.mitems:
          if not b.active:
            b = initBullet(player.x, player.y)
            break

      # Spawn enemies at regular intervals
      spawnTimer += getFrameTime()
      if spawnTimer > 1.5 and enemyCount < MaxEnemies:
        let ex = float32(getRandomValue(20, ScreenWidth - 20))
        enemies[enemyCount] = initEnemy(ex, -15)
        enemyCount.inc
        spawnTimer = 0.0

      # Update all bullets and enemies
      for b in bullets.mitems:
        b.updateBullet()
      for e in enemies.mitems:
        e.updateEnemy()

      # Check bullet-enemy collisions
      for b in bullets.mitems:
        if not b.active: continue
        for e in enemies.mitems:
          if not e.active: continue
          if checkCollision(b.x, b.y, 5, e.x, e.y, e.radius):
            b.active = false
            e.active = false
            score += 10

      # Check player-enemy collisions
      for e in enemies.mitems:
        if not e.active: continue
        if checkCollision(player.x, player.y, player.radius, e.x, e.y, e.radius):
          state = gsGameOver

    of gsGameOver:
      # Return to menu on SPACE
      if isKeyPressed(KEY_SPACE):
        state = gsMenu

    # Rendering phase
    beginDrawing()
    defer endDrawing()

    clearBackground(Black)

    # Draw elements based on current state
    case state
    of gsMenu:
      drawText("Nim Game", 300, 200, 40, White)
      drawText("Press SPACE to start", 280, 280, 20, LightGray)
    of gsPlaying:
      player.drawPlayer()
      for b in bullets:
        b.drawBullet()
      for e in enemies:
        e.drawEnemy()
      drawText(fmt"Score: {score}", 10, 10, 20, White)
      drawFPS(ScreenWidth - 100, 10)
    of gsGameOver:
      drawText("Game Over!", 280, 200, 40, Red)
      drawText(fmt"Score: {score}", 320, 270, 30, White)
      drawText("Press SPACE to restart", 280, 330, 20, LightGray)

when isMainModule:
  gameLoop()
