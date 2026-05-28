package game.libgdx;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Input;
import com.badlogic.gdx.backends.lwjgl3.Lwjgl3Application;
import com.badlogic.gdx.backends.lwjgl3.Lwjgl3ApplicationConfiguration;
import com.badlogic.gdx.graphics.GL20;
import com.badlogic.gdx.graphics.OrthographicCamera;
import com.badlogic.gdx.graphics.glutils.ShapeRenderer;
import com.badlogic.gdx.math.MathUtils;

import java.util.ArrayList;
import java.util.Iterator;

public final class Main implements ApplicationListener {

    private OrthographicCamera camera;
    private ShapeRenderer renderer;
    private final ArrayList<Entity> entities = new ArrayList<>();
    private float spawnTimer;

    public static void main(String... args) {
        var config = new Lwjgl3ApplicationConfiguration();
        config.setTitle("LibGDX Game");
        config.setWindowedMode(800, 600);
        new Lwjgl3Application(new Main(), config);
    }

    @Override
    public void create() {
        camera = new OrthographicCamera();
        camera.setToOrtho(false, 800, 600);
        renderer = new ShapeRenderer();
    }

    @Override
    public void render() {
        Gdx.gl.glClear(GL20.GL_COLOR_BUFFER_BIT);

        handleInput();
        update(Gdx.graphics.getDeltaTime());

        camera.update();
        renderer.setProjectionMatrix(camera.combined);
        renderer.begin(ShapeRenderer.ShapeType.Filled);

        for (var entity : entities) {
            renderer.setColor(entity.color());
            renderer.circle(entity.x(), entity.y(), entity.radius());
        }

        renderer.end();
    }

    private void handleInput() {
        if (Gdx.input.isKeyJustPressed(Input.Keys.SPACE)) {
            for (int i = 0; i < 5; i++) {
                entities.add(new Entity(
                    MathUtils.random(800), MathUtils.random(600),
                    MathUtils.random(-200, 200), MathUtils.random(-200, 200),
                    10 + MathUtils.random(20),
                    MathUtils.random()
                ));
            }
        }
    }

    private void update(float delta) {
        Iterator<Entity> iter = entities.iterator();
        while (iter.hasNext()) {
            var e = iter.next();
            var x = e.x() + e.vx() * delta;
            var y = e.y() + e.vy() * delta;
            if (x < 0 || x > 800 || y < 0 || y > 600) {
                iter.remove();
                continue;
            }
            e = new Entity(x, y, e.vx(), e.vy(), e.radius(), e.color());
            iter.remove();
            entities.add(e);
        }
    }

    @Override
    public void resize(int width, int height) {}
    @Override
    public void pause() {}
    @Override
    public void resume() {}
    @Override
    public void dispose() { renderer.dispose(); }
}
