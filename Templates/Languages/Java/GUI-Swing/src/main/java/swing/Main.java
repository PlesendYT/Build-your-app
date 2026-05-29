/**
 * Swing application entry point: creates the main frame on the EDT.
 */
package swing;

import javax.swing.*;
import java.awt.*;

/** Launches the Swing desktop application. */
public final class Main {

    public static void main(String... args) {
        SwingUtilities.invokeLater(() -> {
            var frame = new MainFrame("Swing App");
            frame.setVisible(true);
        });
    }
}
