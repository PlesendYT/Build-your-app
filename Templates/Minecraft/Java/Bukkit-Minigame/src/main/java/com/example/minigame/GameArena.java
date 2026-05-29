package com.example.minigame;

import org.bukkit.Location;
import org.bukkit.World;
import java.util.ArrayList;
import java.util.List;

/**
 * Manages the arena configuration for a minigame instance.
 * Handles spawn points, boundaries, and world reference.
 */
public class GameArena {

    private final String name;
    private final World world;
    private final List<Location> spawnPoints;
    private Location lobbySpawn;

    /**
     * Creates a new arena with the specified name and world.
     *
     * @param name  The display name of the arena.
     * @param world The Bukkit world this arena exists in.
     */
    public GameArena(String name, World world) {
        this.name = name;
        this.world = world;
        this.spawnPoints = new ArrayList<>();
    }

    /** @return The arena display name. */
    public String getName() { return name; }

    /** @return The world the arena is located in. */
    public World getWorld() { return world; }

    /** @return The list of player spawn locations. */
    public List<Location> getSpawnPoints() { return spawnPoints; }

    /**
     * Adds a spawn point to the arena.
     *
     * @param location The location to add as a spawn.
     */
    public void addSpawnPoint(Location location) {
        spawnPoints.add(location);
    }

    /**
     * Gets the lobby spawn location where players wait before the game.
     *
     * @return The lobby spawn location.
     */
    public Location getLobbySpawn() { return lobbySpawn; }

    /**
     * Sets the lobby spawn location.
     *
     * @param location The lobby spawn location.
     */
    public void setLobbySpawn(Location location) { this.lobbySpawn = location; }
}
