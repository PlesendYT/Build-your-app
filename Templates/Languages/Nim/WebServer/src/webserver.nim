import std/[asyncdispatch, json, strutils, tables, times]
import jester

type
  Todo = ref object
    id: int
    title: string
    completed: bool

var
  todos: seq[Todo]
  nextId: int = 1

proc initData() =
  todos.add(Todo(id: nextId; inc nextId; title: "Learn Nim"; completed: false))
  todos.add(Todo(id: nextId; inc nextId; title: "Build a web server"; completed: false))
  todos.add(Todo(id: nextId; inc nextId; title: "Deploy to production"; completed: true))

proc todoToJson(t: Todo): JsonNode =
  result = %*{
    "id": t.id,
    "title": t.title,
    "completed": t.completed
  }

proc jsonToTodo(j: JsonNode): Todo =
  Todo(
    id: nextId; inc nextId;
    title: j["title"].getStr("");
    completed: j.getOrDefault("completed").getBool(false)
  )

proc handleCors(): string =
  result = ""

settings:
  port = Port(8080)
  staticDir = getCurrentDir() / "public"

routes:
  get "/":
    resp "Nim WebServer - Jester"

  get "/health":
    var health = %*{
      "status": "ok",
      "version": "1.0.0",
      "uptime": epochTime()
    }
    resp health

  get "/api/todos":
    var arr = newJArray()
    for t in todos:
      arr.add(todoToJson(t))
    resp arr

  post "/api/todos":
    try:
      let body = parseJson(request.body)
      let todo = jsonToTodo(body)
      todos.add(todo)
      resp Http201, todoToJson(todo)
    except:
      resp Http400, %*{"error": "Invalid JSON"}

  get "/api/todos/@id":
    let id = parseInt(@"id")
    for t in todos:
      if t.id == id:
        resp todoToJson(t)
    resp Http404, %*{"error": "Not found"}

  put "/api/todos/@id":
    try:
      let id = parseInt(@"id")
      let body = parseJson(request.body)
      for t in todos.mitems:
        if t.id == id:
          if body.hasKey("title"):
            t.title = body["title"].getStr(t.title)
          if body.hasKey("completed"):
            t.completed = body["completed"].getBool(t.completed)
          resp todoToJson(t)
      resp Http404, %*{"error": "Not found"}
    except:
      resp Http400, %*{"error": "Invalid JSON"}

  delete "/api/todos/@id":
    let id = parseInt(@"id")
    for i, t in todos:
      if t.id == id:
        todos.delete(i)
        resp Http204
    resp Http404, %*{"error": "Not found"}

  get "/hello/@name":
    resp "Hello, " & @"name" & "!"

  error Http404:
    resp %*{"error": "Not found", "path": request.path}

proc main() =
  initData()
  echo "Server starting on http://localhost:8080"
  runForever()

when isMainModule:
  main()
