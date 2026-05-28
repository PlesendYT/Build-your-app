defmodule Webapp.Router do
  use Webapp, :router

  pipeline :browser do
    plug :accepts, ["html"]
    plug :fetch_session
    plug :fetch_live_flash
    plug :put_root_layout, {Webapp.LayoutView, :root}
  end

  pipeline :api do
    plug :accepts, ["json"]
  end

  scope "/", Webapp do
    pipe_through :browser
    get "/", PageController, :index
    get "/about", PageController, :about
  end

  scope "/api", Webapp do
    pipe_through :api
    resources "/items", ItemController, except: [:new, :edit]
  end
end
