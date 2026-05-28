import nigui

type
  AppModel = object
    items: seq[string]
    selectedIndex: int

var model = AppModel(items: @["Item 1", "Item 2", "Item 3"], selectedIndex: -1)

proc initApp() =
  app.init()

  let window = newWindow("Nim GUI App")
  window.width = 600
  window.height = 400

  let mainContainer = newLayoutContainer(Layout_Vertical)
  window.add(mainContainer)

  let topContainer = newLayoutContainer(Layout_Horizontal)
  topContainer.height = 200
  mainContainer.add(topContainer)

  let listBox = newListBox()
  listBox.addItems(model.items)
  topContainer.add(listBox)

  let buttonContainer = newLayoutContainer(Layout_Vertical)
  buttonContainer.width = 100
  topContainer.add(buttonContainer)

  let addBtn = newButton("Add")
  buttonContainer.add(addBtn)

  let editBtn = newButton("Edit")
  buttonContainer.add(editBtn)

  let deleteBtn = newButton("Delete")
  buttonContainer.add(deleteBtn)

  let detailContainer = newLayoutContainer(Layout_Vertical)
  mainContainer.add(detailContainer)

  let detailLabel = newLabel("Select an item from the list")
  detailLabel.fontSize = 12
  detailContainer.add(detailLabel)

  let textArea = newTextBox()
  textArea.multiline = true
  textArea.height = 100
  detailContainer.add(textArea)

  listBox.onSelect = proc(event: SelectEvent) =
    let idx = listBox.selectedIndex
    if idx >= 0:
      model.selectedIndex = idx
      textArea.text = model.items[idx]
      detailLabel.text = "Selected: " & model.items[idx]

  addBtn.onClick = proc(event: ClickEvent) =
    let count = model.items.len + 1
    model.items.add("Item " & $count)
    listBox.addItems(@["Item " & $count])
    listBox.selectedIndex = model.items.len - 1

  deleteBtn.onClick = proc(event: ClickEvent) =
    if model.selectedIndex >= 0 and model.selectedIndex < model.items.len:
      model.items.delete(model.selectedIndex)
      listBox.clear()
      listBox.addItems(model.items)
      textArea.text = ""
      detailLabel.text = "Select an item from the list"
      model.selectedIndex = -1

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
