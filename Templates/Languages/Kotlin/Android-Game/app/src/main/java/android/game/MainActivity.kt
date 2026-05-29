/**
 * Simple Android game using SurfaceView with a custom game loop.
 * Balls bounce around the screen; tapping one removes it.
 */
package android.game

import android.app.Activity
import android.graphics.*
import android.os.Bundle
import android.view.MotionEvent
import android.view.SurfaceHolder
import android.view.SurfaceView
import kotlinx.coroutines.*
import java.util.Random
import kotlin.collections.ArrayList

/** Data class representing a moving ball with position, velocity, radius, and color. */
data class Ball(var x: Float, var y: Float, var vx: Float, var vy: Float, val radius: Float, val color: Int)

/** Main activity that hosts the GameView SurfaceView. */
class MainActivity : Activity() {
    private lateinit var gameView: GameView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        gameView = GameView(this)
        setContentView(gameView)
    }

    // Start/resume the game thread when the activity is in the foreground
    override fun onResume() { super.onResume(); gameView.resume() }
    // Pause the game thread when the activity goes to the background
    override fun onPause() { super.onPause(); gameView.pause() }
}

/** Custom SurfaceView that runs its own game loop in a separate thread. */
class GameView(activity: MainActivity) : SurfaceView(activity), Runnable {
    private val holder = holder
    private var thread: Thread? = null
    private var running = false
    private val paint = Paint()
    private val balls = ArrayList<Ball>()
    private val rng = Random()

    // Initialize with 5 balls at random positions and velocities
    init {
        repeat(5) {
            balls.add(Ball(
                rng.nextFloat() * 800, rng.nextFloat() * 600,
                rng.nextFloat() * 200 - 100, rng.nextFloat() * 200 - 100,
                20 + rng.nextFloat() * 20,
                Color.rgb(rng.nextInt(256), rng.nextInt(256), rng.nextInt(256))
            ))
        }
    }

    /** Starts the game loop thread. */
    fun resume() { running = true; thread = Thread(this); thread?.start() }
    /** Stops the game loop and waits for the thread to finish. */
    fun pause() { running = false; thread?.join() }

    // Remove balls that are tapped (touch inside the ball's radius)
    override fun onTouchEvent(event: MotionEvent): Boolean {
        if (event.action == MotionEvent.ACTION_DOWN) {
            balls.removeAll { b ->
                val dx = event.x - b.x; val dy = event.y - b.y
                dx * dx + dy * dy < b.radius * b.radius
            }
        }
        return true
    }

    /** Main game loop: update state, draw, and sleep for ~60 FPS. */
    override fun run() {
        while (running) {
            val canvas = holder.lockCanvas() ?: continue
            update()
            draw(canvas)
            holder.unlockCanvasAndPost(canvas)
            Thread.sleep(16)
        }
    }

    /** Update ball positions and remove out-of-bounds balls; add new ones if count drops below 3. */
    private fun update() {
        val w = width.toFloat(); val h = height.toFloat()
        balls.removeAll { b ->
            b.x += b.vx * 0.016f; b.y += b.vy * 0.016f
            b.x < 0 || b.x > w || b.y < 0 || b.y > h
        }
        if (balls.size < 3) {
            balls.add(Ball(
                rng.nextFloat() * w, rng.nextFloat() * h,
                rng.nextFloat() * 200 - 100, rng.nextFloat() * 200 - 100,
                15 + rng.nextFloat() * 15,
                Color.rgb(rng.nextInt(256), rng.nextInt(256), rng.nextInt(256))
            ))
        }
    }

    /** Render all balls as filled circles on a black background. */
    private fun draw(canvas: Canvas) {
        canvas.drawColor(Color.BLACK)
        for (b in balls) {
            paint.color = b.color
            canvas.drawCircle(b.x, b.y, b.radius, paint)
        }
    }
}
