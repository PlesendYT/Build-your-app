package main

import (
	"image/color"
	"log"

	"github.com/hajimehoshi/ebiten/v2"
	"github.com/hajimehoshi/ebiten/v2/ebitenutil"
	"github.com/hajimehoshi/ebiten/v2/inpututil"
)

type Game struct {
	playerX, playerY float64
	playerSpeed      float64
	bullets          []Bullet
	enemies          []Enemy
	score            int
}

type Bullet struct {
	X, Y   float64
	SpeedY float64
}

type Enemy struct {
	X, Y float64
}

func (g *Game) Update() error {
	// Player movement
	if ebiten.IsKeyPressed(ebiten.KeyArrowLeft) {
		g.playerX -= g.playerSpeed
	}
	if ebiten.IsKeyPressed(ebiten.KeyArrowRight) {
		g.playerX += g.playerSpeed
	}
	if ebiten.IsKeyPressed(ebiten.KeyArrowUp) {
		g.playerY -= g.playerSpeed
	}
	if ebiten.IsKeyPressed(ebiten.KeyArrowDown) {
		g.playerY += g.playerSpeed
	}

	// Shoot
	if inpututil.IsKeyJustPressed(ebiten.KeySpace) {
		g.bullets = append(g.bullets, Bullet{X: g.playerX + 20, Y: g.playerY, SpeedY: -5})
	}

	// Update bullets
	var activeBullets []Bullet
	for _, b := range g.bullets {
		b.Y += b.SpeedY
		if b.Y > 0 {
			activeBullets = append(activeBullets, b)
		}
	}
	g.bullets = activeBullets

	// Spawn enemies
	for i := range g.enemies {
		g.enemies[i].Y += 2
	}

	// Collision detection
	var activeEnemies []Enemy
	for _, e := range g.enemies {
		hit := false
		var remainingBullets []Bullet
		for _, b := range g.bullets {
			if abs(b.X-e.X) < 15 && abs(b.Y-e.Y) < 15 {
				hit = true
				g.score++
				continue
			}
			remainingBullets = append(remainingBullets, b)
		}
		g.bullets = remainingBullets
		if !hit && e.Y < 600 {
			activeEnemies = append(activeEnemies, e)
		}
	}
	g.enemies = activeEnemies

	return nil
}

func (g *Game) Draw(screen *ebiten.Image) {
	screen.Fill(color.RGBA{0x10, 0x10, 0x20, 0xff})

	// Player
	ebitenutil.DrawRect(screen, g.playerX, g.playerY, 40, 40, color.RGBA{0x00, 0xcc, 0xff, 0xff})

	// Bullets
	for _, b := range g.bullets {
		ebitenutil.DrawRect(screen, b.X, b.Y, 8, 16, color.RGBA{0xff, 0xff, 0x00, 0xff})
	}

	// Enemies
	for _, e := range g.enemies {
		ebitenutil.DrawCircle(screen, e.X+15, e.Y+15, 15, color.RGBA{0xff, 0x33, 0x33, 0xff})
	}

	// Score
	ebitenutil.DebugPrint(screen, "Score: "+itoa(g.score))
}

func (g *Game) Layout(outsideWidth, outsideHeight int) (int, int) {
	return 800, 600
}

func abs(x float64) float64 {
	if x < 0 {
		return -x
	}
	return x
}

func itoa(n int) string {
	if n == 0 {
		return "0"
	}
	var buf [12]byte
	i := len(buf)
	for n > 0 {
		i--
		buf[i] = byte('0' + n%10)
		n /= 10
	}
	return string(buf[i:])
}

func main() {
	ebiten.SetWindowSize(800, 600)
	ebiten.SetWindowTitle("Ebitengine Game")
	g := &Game{
		playerX:     380,
		playerY:     500,
		playerSpeed: 5,
	}
	if err := ebiten.RunGame(g); err != nil {
		log.Fatal(err)
	}
}
