# Mix project definition for the REST API application
defmodule Restapi.MixProject do
  use Mix.Project

  # Project configuration
  def project, do: [app: :restapi, version: "1.0.0", elixir: "~> 1.15", start_permanent: Mix.env() == :prod, deps: deps()]

  # Application configuration
  def application, do: [extra_applications: [:logger], mod: {Restapi, []}]

  # Project dependencies
  defp deps, do: [{:phoenix, "~> 1.7"}, {:jason, "~> 1.4"}, {:plug_cowboy, "~> 2.6"}]
end
