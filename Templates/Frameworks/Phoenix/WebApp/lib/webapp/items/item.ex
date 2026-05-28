defmodule Webapp.Items.Item do
  use Ecto.Schema
  import Ecto.Changeset

  schema "items" do
    field :title, :string
    field :description, :string
    timestamps()
  end

  def changeset(item, attrs) do
    item
    |> cast(attrs, [:title, :description])
    |> validate_required([:title])
    |> validate_length(:title, max: 200)
  end
end
