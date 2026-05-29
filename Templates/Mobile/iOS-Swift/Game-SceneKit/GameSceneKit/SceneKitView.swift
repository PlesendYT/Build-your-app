import SwiftUI
import SceneKit

/// A SwiftUI wrapper around SceneKit's `SCNView` that displays a 3D scene
/// with a rotating box and sphere.
struct SceneKitView: UIViewRepresentable {
    /// Creates the underlying `SCNView` and configures it with a 3D scene.
    func makeUIView(context: Context) -> SCNView {
        let scnView = SCNView()
        scnView.scene = createScene()
        // Enable user camera control (drag to orbit, pinch to zoom).
        scnView.allowsCameraControl = true
        // Automatically add default lighting to the scene.
        scnView.autoenablesDefaultLighting = true
        scnView.backgroundColor = .systemBackground
        return scnView
    }

    /// No-op update; the view does not need to change after creation.
    func updateUIView(_ uiView: SCNView, context: Context) {}

    /// Builds and returns a 3D scene with a camera, lights, a box, and a sphere.
    private func createScene() -> SCNScene {
        let scene = SCNScene()

        // Position the camera looking toward the origin.
        let cameraNode = SCNNode()
        cameraNode.camera = SCNCamera()
        cameraNode.position = SCNVector3(0, 0, 8)
        scene.rootNode.addChildNode(cameraNode)

        // Soft ambient light to fill shadows.
        let ambientLight = SCNNode()
        ambientLight.light = SCNLight()
        ambientLight.light?.type = .ambient
        ambientLight.light?.color = UIColor.gray
        scene.rootNode.addChildNode(ambientLight)

        // Directional light from above-right for depth.
        let directionalLight = SCNNode()
        directionalLight.light = SCNLight()
        directionalLight.light?.type = .directional
        directionalLight.position = SCNVector3(5, 5, 5)
        directionalLight.eulerAngles = SCNVector3(-Float.pi / 4, Float.pi / 4, 0)
        scene.rootNode.addChildNode(directionalLight)

        // Blue box on the left side.
        let box = SCNNode(geometry: SCNBox(width: 1.5, height: 1.5, length: 1.5, chamferRadius: 0.1))
        box.geometry?.firstMaterial?.diffuse.contents = UIColor.systemBlue
        box.position = SCNVector3(-2, 0, 0)
        scene.rootNode.addChildNode(box)

        // Green sphere on the right side.
        let sphere = SCNNode(geometry: SCNSphere(radius: 1.0))
        sphere.geometry?.firstMaterial?.diffuse.contents = UIColor.systemGreen
        sphere.position = SCNVector3(2, 0, 0)
        scene.rootNode.addChildNode(sphere)

        // Continuous rotation animations: box spins on Y, sphere spins on X.
        let rotateBox = SCNAction.rotateBy(x: 0, y: 2 * .pi, z: 0, duration: 4)
        let rotateSphere = SCNAction.rotateBy(x: 2 * .pi, y: 0, z: 0, duration: 3)
        box.runAction(SCNAction.repeatForever(rotateBox))
        sphere.runAction(SCNAction.repeatForever(rotateSphere))

        return scene
    }
}

#Preview {
    SceneKitView()
}
