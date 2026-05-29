package com.example.minigame;

import org.bukkit.Bukkit;
import org.bukkit.entity.Player;
import org.bukkit.event.EventHandler;
import org.bukkit.event.Listener;
import org.bukkit.event.player.PlayerJoinEvent;
import org.bukkit.event.player.PlayerQuitEvent;
import org.bukkit.plugin.java.JavaPlugin;
import java.util.HashSet;
import java.util.Set;

/**
 * Main minigame plugin class.
 * Manages the game state machine and player lifecycle.
 */
public class MinigamePlugin extends JavaPlugin implements Listener {

    private GameState currentState;
    private GameArena arena;
    private final Set<Player> players = new HashSet<>();

    @Override
    public void onEnable() {
        // Register event listeners
        Bukkit.getPluginManager().registerEvents(this, this);

        // Initialise the game in the lobby state
        currentState = GameState.LOBBY;
        arena = new GameArena("main-arena", Bukkit.getWorlds().get(0));

        getLogger().info("MinigamePlugin enabled! State: " + currentState);
    }

    @Override
    public void onDisable() {
        // Clear all players on shutdown
        players.clear();
        getLogger().info("MinigamePlugin disabled!");
    }

    /**
     * Adds a player to the game when they join the server.
     *
     * @param event The player join event.
     */
    @EventHandler
    public void onPlayerJoin(PlayerJoinEvent event) {
        Player player = event.getPlayer();
        players.add(player);
        player.teleport(arena.getLobbySpawn());
        player.sendMessage("You joined the minigame lobby!");
    }

    /**
     * Removes a player from the game when they leave.
     *
     * @param event The player quit event.
     */
    @EventHandler
    public void onPlayerQuit(PlayerQuitEvent event) {
        players.remove(event.getPlayer());
    }

    /**
     * Transitions the game to a new state and broadcasts the change.
     *
     * @param newState The state to transition to.
     */
    public void setState(GameState newState) {
        this.currentState = newState;
        Bukkit.broadcastMessage("Game state changed to: " + newState);
        getLogger().info("State transitioned to " + newState);
    }

    /** @return The current game state. */
    public GameState getState() { return currentState; }

    /** @return An unmodifiable view of the player set. */
    public Set<Player> getPlayers() { return Set.copyOf(players); }
}
