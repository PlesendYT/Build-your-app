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

data class Ball(var x: Float, var y: Float, var vx: Float, var vy: Float, val radius: Float, val color: Int)

class MainActivity : Activity() {
    private lateinit var gameView: GameView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        gameView = GameView(this)
        setContentView(gameView)
    }

    override fun onResume() { super.onResume(); gameView.resume() }
    override fun onPause() { super.onPause(); gameView.pause() }
}

class GameView(activity: MainActivity) : SurfaceView(activity), Runnable {
    private val holder = holder
    private var thread: Thread? = null
    private var running = false
    private val paint = Paint()
    private val balls = ArrayList<Ball>()
    private val rng = Random()

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

    fun resume() { running = true; thread = Thread(this); thread?.start() }
    fun pause() { running = false; thread?.join() }

    override fun onTouchEvent(event: MotionEvent): Boolean {
        if (event.action == MotionEvent.ACTION_DOWN) {
            balls.removeAll { b ->
                val dx = event.x - b.x; val dy = event.y - b.y
                dx * dx + dy * dy < b.radius * b.radius
            }
        }
        return true
    }

    override fun run() {
        while (running) {
            val canvas = holder.lockCanvas() ?: continue
            update()
            draw(canvas)
            holder.unlockCanvasAndPost(canvas)
            Thread.sleep(16)
        }
    }

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

    private fun draw(canvas: Canvas) {
        canvas.drawColor(Color.BLACK)
        for (b in balls) {
            paint.color = b.color
            canvas.drawCircle(b.x, b.y, b.radius, paint)
        }
    }
}
