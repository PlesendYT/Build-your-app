/**
 * JavaFX application entry point: loads the FXML layout and shows the stage.
 */
package javafx;

import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.stage.Stage;

/** Main Application class that bootstraps the JavaFX UI from an FXML file. */
public final class Main extends Application {

    @Override
    public void start(Stage primaryStage) throws Exception {
        var loader = new FXMLLoader(getClass().getResource("/main-view.fxml"));
        var root = (Parent) loader.load();
        primaryStage.setTitle("JavaFX App");
        primaryStage.setScene(new Scene(root, 600, 400));
        primaryStage.show();
    }

    public static void main(String... args) {
        launch(args);
    }
}
