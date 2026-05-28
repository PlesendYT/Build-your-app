defmodule Webapp do
  use Application

  def start(_type, _args) do
    children = [
      Webapp.Repo,
      {Phoenix.PubSub, name: Webapp.PubSub},
      Webapp.Endpoint,
    ]
    opts = [strategy: :one_for_one, name: Webapp.Supervisor]
    Supervisor.start_link(children, opts)
  end

  def config_change(changed, _new, removed) do
    Webapp.Endpoint.config_change(changed, removed)
    :ok
  end
end
