defmodule Webapp.ItemController do
  use Webapp, :controller

  alias Webapp.Items
  alias Webapp.Items.Item

  action_fallback Webapp.FallbackController

  def index(conn, _params) do
    items = Items.list_items()
    render(conn, :index, items: items)
  end

  def create(conn, %{"item" => item_params}) do
    with {:ok, %Item{} = item} <- Items.create_item(item_params) do
      conn
      |> put_status(:created)
      |> render(:show, item: item)
    end
  end

  def show(conn, %{"id" => id}) do
    item = Items.get_item!(id)
    render(conn, :show, item: item)
  end

  def delete(conn, %{"id" => id}) do
    item = Items.get_item!(id)
    with {:ok, %Item{}} <- Items.delete_item(item) do
      send_resp(conn, :no_content, "")
    end
  end
end
