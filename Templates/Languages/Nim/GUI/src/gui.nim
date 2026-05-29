import nigui

## Application state model holding the item list and current selection
type
  AppModel = object
    items: seq[string]
    selectedIndex: int

## Global application model instance initialized with default items
var model = AppModel(items: @["Item 1", "Item 2", "Item 3"], selectedIndex: -1)

## Initializes and runs the main GUI application window
proc initApp() =
  app.init()

  # Create main window with fixed dimensions
  let window = newWindow("Nim GUI App")
  window.width = 600
  window.height = 400

  # Main vertical layout container
  let mainContainer = newLayoutContainer(Layout_Vertical)
  window.add(mainContainer)

  # Top horizontal container for the list box and action buttons
  let topContainer = newLayoutContainer(Layout_Horizontal)
  topContainer.height = 200
  mainContainer.add(topContainer)

  # List box displaying available items
  let listBox = newListBox()
  listBox.addItems(model.items)
  topContainer.add(listBox)

  # Button column: Add, Edit, Delete
  let buttonContainer = newLayoutContainer(Layout_Vertical)
  buttonContainer.width = 100
  topContainer.add(buttonContainer)

  let addBtn = newButton("Add")
  buttonContainer.add(addBtn)

  let editBtn = newButton("Edit")
  buttonContainer.add(editBtn)

  let deleteBtn = newButton("Delete")
  buttonContainer.add(deleteBtn)

  # Detail panel below the top section
  let detailContainer = newLayoutContainer(Layout_Vertical)
  mainContainer.add(detailContainer)

  # Label showing current selection or instructions
  let detailLabel = newLabel("Select an item from the list")
  detailLabel.fontSize = 12
  detailContainer.add(detailLabel)

  # Text area for editing selected item content
  let textArea = newTextBox()
  textArea.multiline = true
  textArea.height = 100
  detailContainer.add(textArea)

  # Handle list item selection
  listBox.onSelect = proc(event: SelectEvent) =
    let idx = listBox.selectedIndex
    if idx >= 0:
      model.selectedIndex = idx
      textArea.text = model.items[idx]
      detailLabel.text = "Selected: " & model.items[idx]

  # Add a new item to the list
  addBtn.onClick = proc(event: ClickEvent) =
    let count = model.items.len + 1
    model.items.add("Item " & $count)
    listBox.addItems(@["Item " & $count])
    listBox.selectedIndex = model.items.len - 1

  # Delete the currently selected item
  deleteBtn.onClick = proc(event: ClickEvent) =
    if model.selectedIndex >= 0 and model.selectedIndex < model.items.len:
      model.items.delete(model.selectedIndex)
      listBox.clear()
      listBox.addItems(model.items)
      textArea.text = ""
      detailLabel.text = "Select an item from the list"
      model.selectedIndex = -1

  # Edit the currently selected item with the text area content
  editBtn.onClick = proc(event: ClickEvent) =
    if model.selectedIndex >= 0 and model.selectedIndex < model.items.len:
      let newVal = textArea.text.strip()
      if newVal.len > 0:
        model.items[model.selectedIndex] = newVal
        listBox.clear()
        listBox.addItems(model.items)
        listBox.selectedIndex = model.selectedIndex
        detailLabel.text = "Updated: " & newVal

  window.show()
  app.run()

when isMainModule:
  initApp()
