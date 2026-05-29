# REST API controller for item CRUD operations
defmodule Restapi.ItemController do
  use Restapi, :controller

  # List all items (returns empty placeholder)
  def index(conn, _params), do: json(conn, [])

  # Create a new item from request params
  def create(conn, params), do: json(conn, params)

  # Show a single item by ID
  def show(conn, %{"id" => id}), do: json(conn, %{id: id})

  # Delete an item and return 204 No Content
  def delete(conn, %{"id" => _id}), do: send_resp(conn, :no_content, "")
end
