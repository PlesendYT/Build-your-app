# LiveView counter component with increment/decrement buttons
defmodule LiveviewWeb.Live.CounterLive do
  use Phoenix.LiveView

  # Initialize the counter to 0 on mount
  def mount(_params, _session, socket) do
    {:ok, assign(socket, :count, 0)}
  end

  # Render the counter UI template
  def render(assigns) do
    ~H"""
    <div>
      <h2>LiveView Counter</h2>
      <p>Count: <%= @count %></p>
      <button phx-click="inc">+1</button>
      <button phx-click="dec">-1</button>
    </div>
    """
  end

  # Handle increment event
  def handle_event("inc", _, socket), do: {:noreply, update(socket, :count, &(&1 + 1))}

  # Handle decrement event
  def handle_event("dec", _, socket), do: {:noreply, update(socket, :count, &(&1 - 1))}
end
