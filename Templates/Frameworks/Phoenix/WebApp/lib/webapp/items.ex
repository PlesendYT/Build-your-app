defmodule Webapp.Items do
  alias Webapp.Items.Item
  alias Webapp.Repo

  def list_items, do: Repo.all(Item)

  def get_item!(id), do: Repo.get!(Item, id)

  def create_item(attrs \\ %{}) do
    %Item{}
    |> Item.changeset(attrs)
    |> Repo.insert()
  end

  def delete_item(%Item{} = item), do: Repo.delete(item)
end
