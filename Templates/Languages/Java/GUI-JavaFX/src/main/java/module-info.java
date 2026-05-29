/**
 * Java module declaration for the JavaFX GUI sample application.
 * Requires JavaFX controls and FXML support; opens the javafx package
 * for FXML reflection and exports it to consumers.
 */
module javafx.app {
    requires javafx.controls;
    requires javafx.fxml;

    opens javafx to javafx.fxml;
    exports javafx;
}
