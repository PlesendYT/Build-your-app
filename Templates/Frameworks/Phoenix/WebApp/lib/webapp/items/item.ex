# Ecto schema and changeset for the items database table
defmodule Webapp.Items.Item do
  use Ecto.Schema
  import Ecto.Changeset

  # Database schema with title and description fields
  schema "items" do
    field :title, :string
    field :description, :string
    timestamps()
  end

  # Changeset for validating item attributes on create/update
  def changeset(item, attrs) do
    item
    |> cast(attrs, [:title, :description])
    |> validate_required([:title])
    |> validate_length(:title, max: 200)
  end
end
