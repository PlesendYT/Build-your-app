/**
 * Desktop-Tool sample: a Swing application with an embedded SQLite database
 * for managing CRUD operations on items.
 */
package desktop.tool;

import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import java.awt.*;
import java.sql.*;
import java.util.Vector;

/**
 * Application entry point. Launches the Swing UI on the EDT.
 */
public final class Main {

    public static void main(String... args) {
        SwingUtilities.invokeLater(() -> {
            var db = new Database("data.db");
            db.initialize();
            var frame = new MainFrame(db);
            frame.setVisible(true);
        });
    }
}

/**
 * Wraps an SQLite connection with execute/query helpers and auto-closes.
 */
final class Database implements AutoCloseable {
    private final Connection conn;

    /** Opens (or creates) the SQLite database at the given path. */
    Database(String path) {
        try {
            conn = DriverManager.getConnection("jdbc:sqlite:" + path);
        } catch (SQLException e) {
            throw new RuntimeException("DB init failed", e);
        }
    }

    /** Creates the items table if it does not exist. */
    void initialize() {
        execute("CREATE TABLE IF NOT EXISTS items (" +
            "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "name TEXT NOT NULL, " +
            "value TEXT)");
    }

    /** Executes a parameterised write statement. */
    void execute(String sql, Object... params) {
        try (var stmt = conn.prepareStatement(sql)) {
            for (int i = 0; i < params.length; i++) stmt.setObject(i + 1, params[i]);
            stmt.execute();
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    /** Runs a parameterised SELECT and returns the result rows. */
    Vector<Vector<Object>> query(String sql, Object... params) {
        var rows = new Vector<Vector<Object>>();
        try (var stmt = conn.prepareStatement(sql)) {
            for (int i = 0; i < params.length; i++) stmt.setObject(i + 1, params[i]);
            var rs = stmt.executeQuery();
            var meta = rs.getMetaData();
            while (rs.next()) {
                var row = new Vector<Object>();
                for (int i = 1; i <= meta.getColumnCount(); i++) row.add(rs.getObject(i));
                rows.add(row);
            }
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
        return rows;
    }

    @Override public void close() throws Exception { conn.close(); }
}

/**
 * Main application window with a table, input fields, and add/delete buttons.
 */
final class MainFrame extends JFrame {
    private final Database db;
    private final DefaultTableModel tableModel = new DefaultTableModel();
    private final JTable table = new JTable(tableModel);
    private final JTextField nameField = new JTextField(20);
    private final JTextField valueField = new JTextField(20);

    /** Constructs the frame, configures the UI, and populates the table. */
    MainFrame(Database db) {
        super("Desktop Tool");
        this.db = db;
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setLayout(new BorderLayout(10, 10));
        setSize(800, 500);
        setLocationRelativeTo(null);

        // Set up table columns and load existing data
        tableModel.addColumn("ID");
        tableModel.addColumn("Name");
        tableModel.addColumn("Wert");
        refreshTable();

        add(new JScrollPane(table), BorderLayout.CENTER);

        // Input panel with name/value fields and action buttons
        var inputPanel = new JPanel(new FlowLayout(FlowLayout.LEFT, 5, 5));
        inputPanel.add(new JLabel("Name:"));
        inputPanel.add(nameField);
        inputPanel.add(new JLabel("Wert:"));
        inputPanel.add(valueField);

        // "Add" button inserts a new row into the database
        var addBtn = new JButton("Hinzufügen");
        addBtn.addActionListener(e -> {
            db.execute("INSERT INTO items (name, value) VALUES (?, ?)",
                nameField.getText(), valueField.getText());
            refreshTable();
            nameField.setText("");
            valueField.setText("");
        });
        inputPanel.add(addBtn);

        // "Delete" button removes the selected row
        var deleteBtn = new JButton("Löschen");
        deleteBtn.addActionListener(e -> {
            var row = table.getSelectedRow();
            if (row >= 0) {
                var id = tableModel.getValueAt(row, 0);
                db.execute("DELETE FROM items WHERE id = ?", id);
                refreshTable();
            }
        });
        inputPanel.add(deleteBtn);

        add(inputPanel, BorderLayout.SOUTH);
    }

    /** Reloads all rows from the database into the table model. */
    private void refreshTable() {
        tableModel.setRowCount(0);
        for (var row : db.query("SELECT * FROM items")) {
            tableModel.addRow(row);
        }
    }
}
