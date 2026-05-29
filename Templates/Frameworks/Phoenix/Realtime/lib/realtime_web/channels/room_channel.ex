# Channel for managing real-time chat in the "room:lobby" topic
defmodule RealtimeWeb.RoomChannel do
  use Phoenix.Channel

  # Allow any client to join the lobby room
  def join("room:lobby", _message, socket) do
    {:ok, socket}
  end

  # Broadcast incoming messages to all subscribers in the room
  def handle_in("new_msg", %{"body" => body}, socket) do
    broadcast!(socket, "new_msg", %{body: body})
    {:noreply, socket}
  end
end
