package com.example.plugin

import org.bukkit.Bukkit
import org.bukkit.entity.Player
import org.bukkit.event.EventHandler
import org.bukkit.event.Listener
import org.bukkit.event.player.PlayerJoinEvent
import org.bukkit.plugin.java.JavaPlugin

/**
 * Main Kotlin-based Paper plugin class.
 * Demonstrates event handling and command registration using idiomatic Kotlin.
 */
class KotlinPlugin : JavaPlugin(), Listener {

    override fun onEnable() {
        // Register events on the plugin manager
        Bukkit.getPluginManager().registerEvents(this, this)

        // Register a simple /ping command using the plugin's command map
        getCommand("ping")?.setExecutor { sender, _, _, _ ->
            sender.sendMessage("Pong!")
            true
        }

        logger.info("KotlinPlugin has been enabled!")
    }

    override fun onDisable() {
        // Cleanup when the plugin stops
        logger.info("KotlinPlugin has been disabled!")
    }

    /**
     * Sends a welcome message to players on join.
     *
     * @param event The join event containing the player.
     */
    @EventHandler
    fun onPlayerJoin(event: PlayerJoinEvent) {
        val player: Player = event.player
        player.sendMessage("Welcome, ${player.name}!")
    }
}
