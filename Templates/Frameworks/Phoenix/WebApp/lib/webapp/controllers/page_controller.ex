# Controller for static page rendering
defmodule Webapp.PageController do
  use Webapp, :controller

  # Render the home/index page
  def index(conn, _params) do
    render(conn, :index)
  end

  # Render the about page
  def about(conn, _params) do
    render(conn, :about)
  end
end
