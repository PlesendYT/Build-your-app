/**
 * Swing GUI sample: a desktop window with a menu, toolbar, split pane,
 * and an input panel.
 */
package swing;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.util.function.Consumer;

/** Main application frame with a text area, list, toolbar, and input field. */
public final class MainFrame extends JFrame {

    private final JTextArea textArea = new JTextArea(20, 50);
    private final JTextField inputField = new JTextField(30);
    private final DefaultListModel<String> listModel = new DefaultListModel<>();

    /** Constructs the frame and lays out all UI components. */
    public MainFrame(String title) {
        super(title);
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setLayout(new BorderLayout(10, 10));

        // Build a simple menu bar with an exit action
        var menuBar = new JMenuBar();
        var fileMenu = new JMenu("Datei");
        fileMenu.add(new JMenuItem(new AbstractAction("Beenden") {
            public void actionPerformed(ActionEvent e) { System.exit(0); }
        }));
        menuBar.add(fileMenu);
        setJMenuBar(menuBar);

        // Toolbar with a single action button
        var toolbar = new JToolBar();
        toolbar.add(new JButton(new AbstractAction("Aktion") {
            public void actionPerformed(ActionEvent e) {
                textArea.append("Aktion ausgeführt\n");
            }
        }));
        add(toolbar, BorderLayout.NORTH);

        // Split pane: text area on left, list on right
        var splitPane = new JSplitPane(JSplitPane.HORIZONTAL_SPLIT,
            new JScrollPane(textArea), new JScrollPane(new JList<>(listModel)));
        splitPane.setResizeWeight(0.6);
        add(splitPane, BorderLayout.CENTER);

        // Bottom input panel with text field and add button
        var bottomPanel = new JPanel(new BorderLayout(5, 5));
        bottomPanel.setBorder(BorderFactory.createEmptyBorder(5, 5, 5, 5));
        inputField.addActionListener(e -> {
            var text = inputField.getText().trim();
            if (!text.isEmpty()) {
                listModel.addElement(text);
                textArea.append("Eingabe: " + text + "\n");
                inputField.setText("");
            }
        });
        bottomPanel.add(inputField, BorderLayout.CENTER);
        var addBtn = new JButton("Hinzufügen");
        addBtn.addActionListener(e -> inputField.postActionEvent());
        bottomPanel.add(addBtn, BorderLayout.EAST);
        add(bottomPanel, BorderLayout.SOUTH);

        pack();
        setLocationRelativeTo(null);
    }
}
