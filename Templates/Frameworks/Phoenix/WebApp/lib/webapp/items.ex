# Context module for managing Item data access and business logic
defmodule Webapp.Items do
  alias Webapp.Items.Item
  alias Webapp.Repo

  # List all items
  def list_items, do: Repo.all(Item)

  # Get a single item by ID, raises if not found
  def get_item!(id), do: Repo.get!(Item, id)

  # Create a new item with optional attributes
  def create_item(attrs \\ %{}) do
    %Item{}
    |> Item.changeset(attrs)
    |> Repo.insert()
  end

  # Delete an item
  def delete_item(%Item{} = item), do: Repo.delete(item)
end
