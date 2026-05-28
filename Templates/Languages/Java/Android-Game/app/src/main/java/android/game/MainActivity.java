package android.game;

import android.app.Activity;
import android.graphics.*;
import android.os.Bundle;
import android.view.MotionEvent;
import android.view.SurfaceHolder;
import android.view.SurfaceView;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.Random;

public final class MainActivity extends Activity {

    private GameView gameView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        gameView = new GameView(this);
        setContentView(gameView);
    }

    @Override
    protected void onResume() {
        super.onResume();
        gameView.resume();
    }

    @Override
    protected void onPause() {
        super.onPause();
        gameView.pause();
    }
}

final class GameView extends SurfaceView implements Runnable {

    private Thread gameThread;
    private volatile boolean running;
    private final SurfaceHolder holder;
    private final Paint paint = new Paint();
    private final Random random = new Random();
    private final ArrayList<Ball> balls = new ArrayList<>();
    private float touchX = -1, touchY = -1;

    public GameView(MainActivity activity) {
        super(activity);
        holder = getHolder();
        for (int i = 0; i < 5; i++) {
            balls.add(new Ball(
                random.nextFloat() * 800,
                random.nextFloat() * 600,
                random.nextFloat() * 200 - 100,
                random.nextFloat() * 200 - 100,
                30 + random.nextFloat() * 30,
                Color.rgb(random.nextInt(256), random.nextInt(256), random.nextInt(256))
            ));
        }
    }

    public void resume() {
        running = true;
        gameThread = new Thread(this);
        gameThread.start();
    }

    public void pause() {
        running = false;
        try { gameThread.join(); } catch (InterruptedException ignored) {}
    }

    @Override
    public boolean onTouchEvent(MotionEvent event) {
        touchX = event.getX();
        touchY = event.getY();
        if (event.getAction() == MotionEvent.ACTION_DOWN) {
            var iter = balls.iterator();
            while (iter.hasNext()) {
                var b = iter.next();
                float dx = touchX - b.x;
                float dy = touchY - b.y;
                if (dx * dx + dy * dy < b.radius * b.radius) {
                    iter.remove();
                    break;
                }
            }
        }
        return true;
    }

    @Override
    public void run() {
        while (running) {
            var canvas = holder.lockCanvas();
            if (canvas != null) {
                update();
                draw(canvas);
                holder.unlockCanvasAndPost(canvas);
            }
            try { Thread.sleep(16); } catch (InterruptedException ignored) {}
        }
    }

    private void update() {
        var width = getWidth();
        var height = getHeight();
        Iterator<Ball> iter = balls.iterator();
        while (iter.hasNext()) {
            var b = iter.next();
            b.x += b.vx * 0.016f;
            b.y += b.vy * 0.016f;
            if (b.x < 0 || b.x > width || b.y < 0 || b.y > height) {
                iter.remove();
            }
        }
        if (balls.size() < 3) {
            balls.add(new Ball(
                random.nextFloat() * width,
                random.nextFloat() * height,
                random.nextFloat() * 200 - 100,
                random.nextFloat() * 200 - 100,
                20 + random.nextFloat() * 20,
                Color.rgb(random.nextInt(256), random.nextInt(256), random.nextInt(256))
            ));
        }
    }

    private void draw(Canvas canvas) {
        canvas.drawColor(Color.BLACK);
        for (var b : balls) {
            paint.setColor(b.color);
            canvas.drawCircle(b.x, b.y, b.radius, paint);
        }
        if (touchX >= 0) {
            paint.setColor(Color.WHITE);
            paint.setTextSize(30);
            canvas.drawText("Berührt: (%.0f, %.0f)".formatted(touchX, touchY), 10, 40, paint);
        }
    }

    record Ball(float x, float y, float vx, float vy, float radius, int color) {}
}
