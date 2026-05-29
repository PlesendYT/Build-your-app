package com.buildyourapp.game2d

import android.os.Bundle
import android.view.SurfaceHolder
import androidx.appcompat.app.AppCompatActivity
import com.buildyourapp.game2d.engine.GameSurfaceView

/** Main entry point for the 2D game template. Manages the game surface view lifecycle. */
class MainActivity : AppCompatActivity() {
    private lateinit var gameView: GameSurfaceView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        // Create the custom game surface and set it as the activity's content view
        gameView = GameSurfaceView(this)
        setContentView(gameView)
    }

    override fun onResume() {
        super.onResume()
        // Resume the game loop when the activity comes to the foreground
        gameView.resume()
    }

    override fun onPause() {
        super.onPause()
        // Pause the game loop when the activity goes to the background
        gameView.pause()
    }
}
