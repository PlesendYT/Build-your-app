package javafx;

import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.control.Label;
import javafx.scene.control.TextField;

public final class MainController {

    @FXML private Label statusLabel;
    @FXML private TextField nameField;

    @FXML
    void onGreet(ActionEvent event) {
        var name = nameField.getText().isBlank() ? "Welt" : nameField.getText();
        statusLabel.setText("Hallo, " + name + "!");
    }

    @FXML
    void onClear(ActionEvent event) {
        nameField.clear();
        statusLabel.setText("Bereit");
    }
}
