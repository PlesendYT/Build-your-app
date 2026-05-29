package com.buildyourapp.game3d

import android.opengl.GLSurfaceView
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity

/** Main entry point for the 3D game template. Configures a GLSurfaceView with OpenGL ES 2.0 and the game renderer. */
class MainActivity : AppCompatActivity() {
    private lateinit var glSurfaceView: GLSurfaceView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        // Configure the GLSurfaceView for OpenGL ES 2.0 with a continuous render mode
        glSurfaceView = GLSurfaceView(this).apply {
            setEGLContextClientVersion(2)
            setRenderer(GameRenderer())
            renderMode = GLSurfaceView.RENDERMODE_CONTINUOUSLY
        }
        setContentView(glSurfaceView)
    }

    override fun onResume() {
        super.onResume()
        glSurfaceView.onResume()
    }

    override fun onPause() {
        super.onPause()
        glSurfaceView.onPause()
    }
}
