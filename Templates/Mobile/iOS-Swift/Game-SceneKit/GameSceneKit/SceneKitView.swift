import SwiftUI
import SceneKit

struct SceneKitView: UIViewRepresentable {
    func makeUIView(context: Context) -> SCNView {
        let scnView = SCNView()
        scnView.scene = createScene()
        scnView.allowsCameraControl = true
        scnView.autoenablesDefaultLighting = true
        scnView.backgroundColor = .systemBackground
        return scnView
    }

    func updateUIView(_ uiView: SCNView, context: Context) {}

    private func createScene() -> SCNScene {
        let scene = SCNScene()

        let cameraNode = SCNNode()
        cameraNode.camera = SCNCamera()
        cameraNode.position = SCNVector3(0, 0, 8)
        scene.rootNode.addChildNode(cameraNode)

        let ambientLight = SCNNode()
        ambientLight.light = SCNLight()
        ambientLight.light?.type = .ambient
        ambientLight.light?.color = UIColor.gray
        scene.rootNode.addChildNode(ambientLight)

        let directionalLight = SCNNode()
        directionalLight.light = SCNLight()
        directionalLight.light?.type = .directional
        directionalLight.position = SCNVector3(5, 5, 5)
        directionalLight.eulerAngles = SCNVector3(-Float.pi / 4, Float.pi / 4, 0)
        scene.rootNode.addChildNode(directionalLight)

        let box = SCNNode(geometry: SCNBox(width: 1.5, height: 1.5, length: 1.5, chamferRadius: 0.1))
        box.geometry?.firstMaterial?.diffuse.contents = UIColor.systemBlue
        box.position = SCNVector3(-2, 0, 0)
        scene.rootNode.addChildNode(box)

        let sphere = SCNNode(geometry: SCNSphere(radius: 1.0))
        sphere.geometry?.firstMaterial?.diffuse.contents = UIColor.systemGreen
        sphere.position = SCNVector3(2, 0, 0)
        scene.rootNode.addChildNode(sphere)

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
