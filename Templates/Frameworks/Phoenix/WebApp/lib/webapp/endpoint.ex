defmodule Webapp.Endpoint do
  use Phoenix.Endpoint, otp_app: :webapp

  socket "/live", Phoenix.LiveView.Socket
  socket "/socket", Webapp.UserSocket

  plug Plug.RequestId
  plug Plug.Telemetry, event_prefix: [:phoenix, :endpoint]

  plug Plug.Parsers,
    parsers: [:urlencoded, :multipart, :json],
    pass: ["*/*"],
    json_decoder: Phoenix.json_library()

  plug Plug.MethodOverride
  plug Plug.Head
  plug Webapp.Router
end
