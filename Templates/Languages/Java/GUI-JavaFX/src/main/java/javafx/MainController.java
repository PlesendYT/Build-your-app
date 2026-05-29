/**
 * FXML controller for the main view: handles greet and clear actions.
 */
package javafx;

import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.control.Label;
import javafx.scene.control.TextField;

/** Controller bound to main-view.fxml with two button actions. */
public final class MainController {

    @FXML private Label statusLabel;
    @FXML private TextField nameField;

    /** Reads the name field and displays a greeting in the status label. */
    @FXML
    void onGreet(ActionEvent event) {
        var name = nameField.getText().isBlank() ? "Welt" : nameField.getText();
        statusLabel.setText("Hallo, " + name + "!");
    }

    /** Clears the input field and resets the status label. */
    @FXML
    void onClear(ActionEvent event) {
        nameField.clear();
        statusLabel.setText("Bereit");
    }
}
