import AppKit

@main
final class AppDelegate: NSObject, NSApplicationDelegate {
    private var window: NSWindow!
    private var splitViewController: NSSplitViewController!

    func applicationDidFinishLaunching(_ notification: Notification) {
        let contentVC = ContentViewController()
        let inspectorVC = InspectorViewController()

        splitViewController = NSSplitViewController()
        splitViewController.addSplitViewItem(NSSplitViewItem(sidebarWithViewController: contentVC))
        splitViewController.addSplitViewItem(NSSplitViewItem(contentListWithViewController: inspectorVC))

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

    func applicationWillTerminate(_ notification: Notification) {
        // Cleanup
    }
}

final class ContentViewController: NSViewController {
    private var tableView: NSTableView!
    private var items: [String] = ["Item 1", "Item 2", "Item 3"]

    override func loadView() {
        let scrollView = NSScrollView()
        scrollView.hasVerticalScroller = true

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

    @objc private func addItem() {
        items.append("Item \(items.count + 1)")
        tableView.reloadData()
    }
}

extension ContentViewController: NSTableViewDataSource, NSTableViewDelegate {
    func numberOfRows(in tableView: NSTableView) -> Int { items.count }

    func tableView(_ tableView: NSTableView, viewFor tableColumn: NSTableColumn?, row: Int) -> NSView? {
        let cell = NSTextField(labelWithString: items[row])
        cell.isSelectable = false
        return cell
    }
}

final class InspectorViewController: NSViewController {
    private var label: NSTextField!

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
