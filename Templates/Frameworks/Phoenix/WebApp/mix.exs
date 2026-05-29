# Mix project definition for the Phoenix web application
defmodule Webapp.MixProject do
  use Mix.Project

  # Project configuration
  def project do
    [
      app: :webapp,
      version: "1.0.0",
      elixir: "~> 1.15",
      start_permanent: Mix.env() == :prod,
      deps: deps()
    ]
  end

  # Application configuration with supervisor tree
  def application do
    [extra_applications: [:logger], mod: {Webapp, []}]
  end

  # Project dependencies including Phoenix, LiveView, and Ecto
  defp deps do
    [
      {:phoenix, "~> 1.7"},
      {:phoenix_live_view, "~> 0.20"},
      {:phoenix_html, "~> 3.3"},
      {:phoenix_live_reload, "~> 1.4", only: :dev},
      {:phoenix_ecto, "~> 4.4"},
      {:ecto_sql, "~> 3.10"},
      {:postgrex, ">= 0.0.0"},
      {:jason, "~> 1.4"},
      {:plug_cowboy, "~> 2.6"},
    ]
  end
end
