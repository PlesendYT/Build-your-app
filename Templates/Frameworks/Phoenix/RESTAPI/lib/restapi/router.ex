# Router defining API endpoints for the REST API
defmodule Restapi.Router do
  use Phoenix.Router

  # API pipeline accepting JSON requests
  pipeline :api do
    plug :accepts, ["json"]
  end

  # API routes scoped under /api
  scope "/api", Restapi do
    pipe_through :api
    get "/items", ItemController, :index
    post "/items", ItemController, :create
    get "/items/:id", ItemController, :show
    delete "/items/:id", ItemController, :delete
  end
end
