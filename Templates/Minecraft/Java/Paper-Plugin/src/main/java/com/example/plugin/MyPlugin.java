package com.example.plugin;

import org.bukkit.Bukkit;
import org.bukkit.command.Command;
import org.bukkit.command.CommandSender;
import org.bukkit.entity.Player;
import org.bukkit.event.EventHandler;
import org.bukkit.event.Listener;
import org.bukkit.event.player.PlayerJoinEvent;
import org.bukkit.plugin.java.JavaPlugin;

/**
 * Main plugin class for the Paper example plugin.
 * Registers events and commands on startup.
 */
public class MyPlugin extends JavaPlugin implements Listener {

    @Override
    public void onEnable() {
        // Register this class as an event listener
        Bukkit.getPluginManager().registerEvents(this, this);

        // Log a message to confirm the plugin has started
        getLogger().info("MyPlugin has been enabled!");
    }

    @Override
    public void onDisable() {
        // Cleanup tasks when the plugin stops
        getLogger().info("MyPlugin has been disabled!");
    }

    /**
     * Sends a welcome message to players when they join the server.
     *
     * @param event The join event containing the player reference.
     */
    @EventHandler
    public void onPlayerJoin(PlayerJoinEvent event) {
        Player player = event.getPlayer();
        player.sendMessage("Welcome to the server, " + player.getName() + "!");
    }

    @Override
    public boolean onCommand(CommandSender sender, Command command, String label, String[] args) {
        // Handle the /hello command
        if (command.getName().equalsIgnoreCase("hello")) {
            if (sender instanceof Player player) {
                player.sendMessage("Hello, " + player.getName() + "!");
            } else {
                sender.sendMessage("Hello, console!");
            }
            return true;
        }
        return false;
    }
}
