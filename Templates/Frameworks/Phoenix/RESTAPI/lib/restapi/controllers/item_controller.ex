defmodule Restapi.ItemController do
  use Restapi, :controller
  def index(conn, _params), do: json(conn, [])
  def create(conn, params), do: json(conn, params)
  def show(conn, %{"id" => id}), do: json(conn, %{id: id})
  def delete(conn, %{"id" => _id}), do: send_resp(conn, :no_content, "")
end
