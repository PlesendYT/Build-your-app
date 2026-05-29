/**
 * JMonkeyEngine 3D game sample: move a cube with WASD keys.
 */
package game.jmonkey;

import com.jme3.app.SimpleApplication;
import com.jme3.material.Material;
import com.jme3.math.ColorRGBA;
import com.jme3.math.Vector3f;
import com.jme3.scene.Geometry;
import com.jme3.scene.shape.Box;
import com.jme3.scene.shape.Sphere;
import com.jme3.input.KeyInput;
import com.jme3.input.controls.ActionListener;
import com.jme3.input.controls.KeyTrigger;

/** Main game class with a player cube, ground plane, and a sphere obstacle. */
public final class Main extends SimpleApplication {

    private Geometry player;

    public static void main(String... args) {
        var app = new Main();
        app.start();
    }

    /** Initialises the scene geometry, input bindings, and camera. */
    @Override
    public void simpleInitApp() {
        // Create the player cube at the origin
        var box = new Box(1, 1, 1);
        player = new Geometry("Player", box);
        var mat = new Material(assetManager, "Common/MatDefs/Misc/Unshaded.j3md");
        mat.setColor("Color", ColorRGBA.Blue);
        player.setMaterial(mat);
        player.setLocalTranslation(0, 1, 0);
        rootNode.attachChild(player);

        // Create a green ground plane
        var ground = new Geometry("Ground", new Box(10, 0.5f, 10));
        var gmat = new Material(assetManager, "Common/MatDefs/Misc/Unshaded.j3md");
        gmat.setColor("Color", ColorRGBA.Green);
        ground.setMaterial(gmat);
        ground.setLocalTranslation(0, -0.5f, 0);
        rootNode.attachChild(ground);

        // Place a red sphere as a stationary obstacle
        var sphere = new Geometry("Obstacle", new Sphere(16, 16, 0.8f));
        var smat = new Material(assetManager, "Common/MatDefs/Misc/Unshaded.j3md");
        smat.setColor("Color", ColorRGBA.Red);
        sphere.setMaterial(smat);
        sphere.setLocalTranslation(3, 0.8f, 2);
        rootNode.attachChild(sphere);

        // Register WASD key mappings for movement
        inputManager.addMapping("Left", new KeyTrigger(KeyInput.KEY_A));
        inputManager.addMapping("Right", new KeyTrigger(KeyInput.KEY_D));
        inputManager.addMapping("Forward", new KeyTrigger(KeyInput.KEY_W));
        inputManager.addMapping("Backward", new KeyTrigger(KeyInput.KEY_S));

        // Move the player in 2-unit steps and re-centre the camera on it
        ActionListener listener = (name, pressed, tpf) -> {
            if (!pressed) return;
            var pos = player.getLocalTranslation().clone();
            switch (name) {
                case "Left" -> pos.addLocal(-2, 0, 0);
                case "Right" -> pos.addLocal(2, 0, 0);
                case "Forward" -> pos.addLocal(0, 0, -2);
                case "Backward" -> pos.addLocal(0, 0, 2);
            }
            player.setLocalTranslation(pos);
            cam.lookAt(pos, Vector3f.UNIT_Y);
        };

        inputManager.addListener(listener, "Left", "Right", "Forward", "Backward");

        // Position the camera above and behind the scene; disable flyCam
        cam.setLocation(new Vector3f(0, 5, 10));
        cam.lookAt(new Vector3f(0, 0, 0), Vector3f.UNIT_Y);
        flyCam.setEnabled(false);
    }

    @Override
    public void simpleUpdate(float tpf) {}
}
