package com.buildyourapp.game2d.engine

import android.content.Context
import android.graphics.*
import android.view.MotionEvent
import android.view.SurfaceHolder
import android.view.SurfaceView

/**
 * Custom SurfaceView that implements a basic 2D game loop.
 * The player (blue circle) moves toward the last touch position.
 */
class GameSurfaceView(context: Context) : SurfaceView(context), SurfaceHolder.Callback, Runnable {
    private var thread: Thread? = null
    private var isRunning = false
    // Player position and size
    private var playerX = 100f
    private var playerY = 100f
    private val playerSize = 60f
    // Last touch coordinates (null when not touching)
    private var touchX: Float? = null
    private var touchY: Float? = null

    // Paint configuration for the player circle
    private val paint = Paint().apply {
        color = Color.BLUE
        style = Paint.Style.FILL
    }

    init {
        // Register this view as the surface callback
        holder.addCallback(this)
    }

    /** Called when the surface is created; starts the game loop thread. */
    override fun surfaceCreated(holder: SurfaceHolder) {
        isRunning = true
        thread = Thread(this).apply { start() }
    }

    override fun surfaceChanged(holder: SurfaceHolder, format: Int, width: Int, height: Int) {}

    /** Called when the surface is destroyed; stops the game loop and waits for the thread to finish. */
    override fun surfaceDestroyed(holder: SurfaceHolder) {
        isRunning = false
        thread?.join()
    }

    /** Main game loop: runs update and render at ~60 FPS. */
    override fun run() {
        val targetFps = 60
        val frameDuration = 1000L / targetFps

        while (isRunning) {
            val startTime = System.currentTimeMillis()
            update()
            render()
            // Sleep for the remainder of the frame to maintain target FPS
            val elapsed = System.currentTimeMillis() - startTime
            if (elapsed < frameDuration) {
                Thread.sleep(frameDuration - elapsed)
            }
        }
    }

    /** Updates player position by moving toward the current touch point at a fixed speed. */
    private fun update() {
        touchX?.let { tx ->
            touchY?.let { ty ->
                val dx = tx - playerX
                val dy = ty - playerY
                val dist = Math.sqrt((dx * dx + dy * dy).toDouble()).toFloat()
                // Move only if the distance is significant enough
                if (dist > 5f) {
                    playerX += (dx / dist) * 5f
                    playerY += (dy / dist) * 5f
                }
            }
        }
    }

    /** Draws a white background and the player circle onto the canvas. */
    private fun render() {
        val canvas = holder.lockCanvas() ?: return
        canvas.drawColor(Color.WHITE)
        canvas.drawCircle(playerX, playerY, playerSize, paint)
        holder.unlockCanvasAndPost(canvas)
    }

    /** Tracks touch/move to update target position and clears on release. */
    override fun onTouchEvent(event: MotionEvent): Boolean {
        when (event.action) {
            MotionEvent.ACTION_MOVE, MotionEvent.ACTION_DOWN -> {
                touchX = event.x
                touchY = event.y
            }
            MotionEvent.ACTION_UP -> {
                touchX = null
                touchY = null
            }
        }
        return true
    }

    /** Resumes the game loop by starting a new thread if not already running. */
    fun resume() {
        if (!isRunning) {
            isRunning = true
            thread = Thread(this).apply { start() }
        }
    }

    /** Pauses the game loop by stopping the running thread. */
    fun pause() {
        isRunning = false
        thread?.join()
    }
}
