# Router defining browser and API routes for the web application
defmodule Webapp.Router do
  use Webapp, :router

  # Browser pipeline with session and flash support
  pipeline :browser do
    plug :accepts, ["html"]
    plug :fetch_session
    plug :fetch_live_flash
    plug :put_root_layout, {Webapp.LayoutView, :root}
  end

  # API pipeline accepting JSON requests
  pipeline :api do
    plug :accepts, ["json"]
  end

  # Public browser routes
  scope "/", Webapp do
    pipe_through :browser
    get "/", PageController, :index
    get "/about", PageController, :about
  end

  # API routes for item resources
  scope "/api", Webapp do
    pipe_through :api
    resources "/items", ItemController, except: [:new, :edit]
  end
end
