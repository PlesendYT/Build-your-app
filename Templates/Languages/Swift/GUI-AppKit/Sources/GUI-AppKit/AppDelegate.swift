/// Entry point and delegate for the AppKit-based GUI application.
/// Sets up the main window with a split-view layout containing a sidebar and inspector.
import AppKit

@main
final class AppDelegate: NSObject, NSApplicationDelegate {
    private var window: NSWindow!
    private var splitViewController: NSSplitViewController!

    /// Creates the split view controllers, configures the window, and displays it.
    func applicationDidFinishLaunching(_ notification: Notification) {
        let contentVC = ContentViewController()
        let inspectorVC = InspectorViewController()

        // Build split view with sidebar and content panels
        splitViewController = NSSplitViewController()
        splitViewController.addSplitViewItem(NSSplitViewItem(sidebarWithViewController: contentVC))
        splitViewController.addSplitViewItem(NSSplitViewItem(contentListWithViewController: inspectorVC))

        // Configure and show the main window
        window = NSWindow(
            contentRect: NSRect(x: 0, y: 0, width: 800, height: 600),
            styleMask: [.titled, .closable, .miniaturizable, .resizable, .fullSizeContentView],
            backing: .buffered,
            defer: false
        )
        window.title = "AppKit App"
        window.contentViewController = splitViewController
        window.makeKeyAndOrderFront(nil)
        window.center()
    }

    /// Performs cleanup before the application terminates.
    func applicationWillTerminate(_ notification: Notification) {
    }
}

/// Manages the sidebar view displaying a list of items with an Add button.
final class ContentViewController: NSViewController {
    private var tableView: NSTableView!
    private var items: [String] = ["Item 1", "Item 2", "Item 3"]

    /// Builds the table view and button layout programmatically.
    override func loadView() {
        let scrollView = NSScrollView()
        scrollView.hasVerticalScroller = true

        // Configure table view with a single column and no header
        tableView = NSTableView()
        tableView.addTableColumn(NSTableColumn(identifier: NSUserInterfaceItemIdentifier("name")))
        tableView.headerView = nil
        tableView.delegate = self
        tableView.dataSource = self

        scrollView.documentView = tableView
        scrollView.translatesAutoresizingMaskIntoConstraints = false

        let stack = NSStackView(views: [scrollView])
        stack.orientation = .vertical

        let addButton = NSButton(title: "Add", target: self, action: #selector(addItem))
        stack.addView(addButton, in: .trailing)

        view = stack
    }

    /// Appends a new item and refreshes the table.
    @objc private func addItem() {
        items.append("Item \(items.count + 1)")
        tableView.reloadData()
    }
}

/// Provides table view data source and delegate methods for ContentViewController.
extension ContentViewController: NSTableViewDataSource, NSTableViewDelegate {
    func numberOfRows(in tableView: NSTableView) -> Int { items.count }

    func tableView(_ tableView: NSTableView, viewFor tableColumn: NSTableColumn?, row: Int) -> NSView? {
        let cell = NSTextField(labelWithString: items[row])
        cell.isSelectable = false
        return cell
    }
}

/// Displays details for the selected item in the inspector panel.
final class InspectorViewController: NSViewController {
    private var label: NSTextField!

    /// Centers a descriptive label in the inspector view.
    override func loadView() {
        label = NSTextField(wrappingLabelWithString: "Select an item to inspect")
        label.alignment = .center
        label.font = .systemFont(ofSize: 14)
        view = NSView()
        view.addSubview(label)

        NSLayoutConstraint.activate([
            label.centerXAnchor.constraint(equalTo: view.centerXAnchor),
            label.centerYAnchor.constraint(equalTo: view.centerYAnchor),
        ])
    }
}
