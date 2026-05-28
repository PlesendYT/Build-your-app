defmodule Restapi.Router do
  use Phoenix.Router
  pipeline :api do
    plug :accepts, ["json"]
  end
  scope "/api", Restapi do
    pipe_through :api
    get "/items", ItemController, :index
    post "/items", ItemController, :create
    get "/items/:id", ItemController, :show
    delete "/items/:id", ItemController, :delete
  end
end
