defmodule Restapi.MixProject do
  use Mix.Project
  def project, do: [app: :restapi, version: "1.0.0", elixir: "~> 1.15", start_permanent: Mix.env() == :prod, deps: deps()]
  def application, do: [extra_applications: [:logger], mod: {Restapi, []}]
  defp deps, do: [{:phoenix, "~> 1.7"}, {:jason, "~> 1.4"}, {:plug_cowboy, "~> 2.6"}]
end
