# Phoenix HTTP endpoint configuration with LiveView and WebSocket support
defmodule Webapp.Endpoint do
  use Phoenix.Endpoint, otp_app: :webapp

  # LiveView and general WebSocket sockets
  socket "/live", Phoenix.LiveView.Socket
  socket "/socket", Webapp.UserSocket

  # Request logging and telemetry
  plug Plug.RequestId
  plug Plug.Telemetry, event_prefix: [:phoenix, :endpoint]

  # Body parsing for URL-encoded, multipart, and JSON requests
  plug Plug.Parsers,
    parsers: [:urlencoded, :multipart, :json],
    pass: ["*/*"],
    json_decoder: Phoenix.json_library()

  # HTTP method override and head request handling
  plug Plug.MethodOverride
  plug Plug.Head
  # Route all requests through the application router
  plug Webapp.Router
end
