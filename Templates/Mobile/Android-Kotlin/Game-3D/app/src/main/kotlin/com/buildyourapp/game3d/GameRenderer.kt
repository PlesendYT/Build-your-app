package com.buildyourapp.game3d

import android.opengl.GLES20
import android.opengl.GLSurfaceView
import android.opengl.Matrix
import javax.microedition.khronos.egl.EGLConfig
import javax.microedition.khronos.opengles.GL10

/**
 * OpenGL ES 2.0 renderer that draws a rotating color cube.
 * Manages shader compilation, matrix transformations, and per-frame rendering.
 */
class GameRenderer : GLSurfaceView.Renderer {
    // Transformation matrices
    private val modelMatrix = FloatArray(16)
    private val viewMatrix = FloatArray(16)
    private val projectionMatrix = FloatArray(16)
    private val mvpMatrix = FloatArray(16)
    // Current rotation angle for the cube animation
    private var rotationAngle = 0f

    // Shader and program handles
    private var vertexShader: Int = 0
    private var fragmentShader: Int = 0
    private var program: Int = 0

    // Simple vertex shader that applies the MVP matrix
    private val vertexShaderCode = """
        attribute vec4 vPosition;
        uniform mat4 uMVPMatrix;
        void main() {
            gl_Position = uMVPMatrix * vPosition;
        }
    """.trimIndent()

    // Simple fragment shader that outputs a uniform color
    private val fragmentShaderCode = """
        precision mediump float;
        uniform vec4 vColor;
        void main() {
            gl_FragColor = vColor;
        }
    """.trimIndent()

    // Vertex data for a cube (12 triangles, 36 vertices)
    private val cubeVertices = floatArrayOf(
        -0.5f, -0.5f, -0.5f,  0.5f, -0.5f, -0.5f,  0.5f,  0.5f, -0.5f,
        -0.5f, -0.5f, -0.5f,  0.5f,  0.5f, -0.5f, -0.5f,  0.5f, -0.5f,
        -0.5f, -0.5f,  0.5f,  0.5f, -0.5f,  0.5f,  0.5f,  0.5f,  0.5f,
        -0.5f, -0.5f,  0.5f,  0.5f,  0.5f,  0.5f, -0.5f,  0.5f,  0.5f,
        -0.5f, -0.5f, -0.5f, -0.5f,  0.5f, -0.5f, -0.5f,  0.5f,  0.5f,
        -0.5f, -0.5f, -0.5f, -0.5f,  0.5f,  0.5f, -0.5f, -0.5f,  0.5f,
         0.5f, -0.5f, -0.5f,  0.5f,  0.5f, -0.5f,  0.5f,  0.5f,  0.5f,
         0.5f, -0.5f, -0.5f,  0.5f,  0.5f,  0.5f,  0.5f, -0.5f,  0.5f,
        -0.5f, -0.5f, -0.5f,  0.5f, -0.5f, -0.5f,  0.5f, -0.5f,  0.5f,
        -0.5f, -0.5f, -0.5f,  0.5f, -0.5f,  0.5f, -0.5f, -0.5f,  0.5f,
        -0.5f,  0.5f, -0.5f,  0.5f,  0.5f, -0.5f,  0.5f,  0.5f,  0.5f,
        -0.5f,  0.5f, -0.5f,  0.5f,  0.5f,  0.5f, -0.5f,  0.5f,  0.5f
    )

    /** Called once when the surface is created. Compiles shaders and links the GL program. */
    override fun onSurfaceCreated(gl: GL10?, config: EGLConfig?) {
        GLES20.glClearColor(0.1f, 0.1f, 0.2f, 1.0f)

        // Compile vertex shader
        vertexShader = GLES20.glCreateShader(GLES20.GL_VERTEX_SHADER).also { shader ->
            GLES20.glShaderSource(shader, vertexShaderCode)
            GLES20.glCompileShader(shader)
        }

        // Compile fragment shader
        fragmentShader = GLES20.glCreateShader(GLES20.GL_FRAGMENT_SHADER).also { shader ->
            GLES20.glShaderSource(shader, fragmentShaderCode)
            GLES20.glCompileShader(shader)
        }

        // Link vertex and fragment shaders into a program
        program = GLES20.glCreateProgram().also {
            GLES20.glAttachShader(it, vertexShader)
            GLES20.glAttachShader(it, fragmentShader)
            GLES20.glLinkProgram(it)
        }
    }

    /** Called when the surface size changes. Updates the viewport and projection matrix. */
    override fun onSurfaceChanged(gl: GL10?, width: Int, height: Int) {
        GLES20.glViewport(0, 0, width, height)
        val ratio = width.toFloat() / height.toFloat()
        // Set up a perspective projection with frustum
        Matrix.frustumM(projectionMatrix, 0, -ratio, ratio, -1f, 1f, 3f, 7f)
    }

    /** Called every frame. Clears the buffer, computes the MVP matrix, and draws the cube. */
    override fun onDrawFrame(gl: GL10?) {
        GLES20.glClear(GLES20.GL_COLOR_BUFFER_BIT or GLES20.GL_DEPTH_BUFFER_BIT)

        // Set up view matrix (camera at (0,0,3) looking at origin)
        Matrix.setLookAtM(viewMatrix, 0, 0f, 0f, 3f, 0f, 0f, 0f, 0f, 1f, 0f)
        // Build model matrix with rotation
        Matrix.setIdentityM(modelMatrix, 0)
        Matrix.rotateM(modelMatrix, 0, rotationAngle, 0.5f, 1f, 0.3f)
        // Combine view * model, then projection * (view * model)
        Matrix.multiplyMM(mvpMatrix, 0, viewMatrix, 0, modelMatrix, 0)
        Matrix.multiplyMM(mvpMatrix, 0, projectionMatrix, 0, mvpMatrix, 0)

        // Use the linked shader program
        GLES20.glUseProgram(program)
        // Retrieve attribute/uniform locations
        val positionHandle = GLES20.glGetAttribLocation(program, "vPosition")
        val mvpHandle = GLES20.glGetUniformLocation(program, "uMVPMatrix")
        val colorHandle = GLES20.glGetUniformLocation(program, "vColor")

        // Load cube vertex data into a direct FloatBuffer
        val vertexBuffer = java.nio.ByteBuffer
            .allocateDirect(cubeVertices.size * 4)
            .order(java.nio.ByteOrder.nativeOrder())
            .asFloatBuffer()
            .put(cubeVertices)
        vertexBuffer.position(0)

        // Pass vertex positions, MVP matrix, and color to the shader
        GLES20.glVertexAttribPointer(positionHandle, 3, GLES20.GL_FLOAT, false, 0, vertexBuffer)
        GLES20.glEnableVertexAttribArray(positionHandle)
        GLES20.glUniformMatrix4fv(mvpHandle, 1, false, mvpMatrix, 0)
        GLES20.glUniform4f(colorHandle, 0.2f, 0.6f, 1.0f, 1.0f)

        // Draw the cube (36 vertices = 12 triangles)
        GLES20.glDrawArrays(GLES20.GL_TRIANGLES, 0, 36)

        // Increment rotation angle for the next frame
        rotationAngle += 0.5f
    }
}
