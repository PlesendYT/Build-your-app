package com.buildyourapp.game2d.engine

import android.content.Context
import android.graphics.*
import android.view.MotionEvent
import android.view.SurfaceHolder
import android.view.SurfaceView

class GameSurfaceView(context: Context) : SurfaceView(context), SurfaceHolder.Callback, Runnable {
    private var thread: Thread? = null
    private var isRunning = false
    private var playerX = 100f
    private var playerY = 100f
    private val playerSize = 60f
    private var touchX: Float? = null
    private var touchY: Float? = null

    private val paint = Paint().apply {
        color = Color.BLUE
        style = Paint.Style.FILL
    }

    init {
        holder.addCallback(this)
    }

    override fun surfaceCreated(holder: SurfaceHolder) {
        isRunning = true
        thread = Thread(this).apply { start() }
    }

    override fun surfaceChanged(holder: SurfaceHolder, format: Int, width: Int, height: Int) {}

    override fun surfaceDestroyed(holder: SurfaceHolder) {
        isRunning = false
        thread?.join()
    }

    override fun run() {
        val targetFps = 60
        val frameDuration = 1000L / targetFps

        while (isRunning) {
            val startTime = System.currentTimeMillis()
            update()
            render()
            val elapsed = System.currentTimeMillis() - startTime
            if (elapsed < frameDuration) {
                Thread.sleep(frameDuration - elapsed)
            }
        }
    }

    private fun update() {
        touchX?.let { tx ->
            touchY?.let { ty ->
                val dx = tx - playerX
                val dy = ty - playerY
                val dist = Math.sqrt((dx * dx + dy * dy).toDouble()).toFloat()
                if (dist > 5f) {
                    playerX += (dx / dist) * 5f
                    playerY += (dy / dist) * 5f
                }
            }
        }
    }

    private fun render() {
        val canvas = holder.lockCanvas() ?: return
        canvas.drawColor(Color.WHITE)
        canvas.drawCircle(playerX, playerY, playerSize, paint)
        holder.unlockCanvasAndPost(canvas)
    }

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

    fun resume() {
        if (!isRunning) {
            isRunning = true
            thread = Thread(this).apply { start() }
        }
    }

    fun pause() {
        isRunning = false
        thread?.join()
    }
}
