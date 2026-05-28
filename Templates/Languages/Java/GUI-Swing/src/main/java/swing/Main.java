package swing;

import javax.swing.*;
import java.awt.*;

public final class Main {

    public static void main(String... args) {
        SwingUtilities.invokeLater(() -> {
            var frame = new MainFrame("Swing App");
            frame.setVisible(true);
        });
    }
}
