package com.example.proxy;

import com.google.inject.Inject;
import com.velocitypowered.api.event.Subscribe;
import com.velocitypowered.api.event.connection.PostLoginEvent;
import com.velocitypowered.api.event.player.ServerPostConnectEvent;
import com.velocitypowered.api.command.CommandManager;
import com.velocitypowered.api.command.CommandSource;
import com.velocitypowered.api.command.SimpleCommand;
import com.velocitypowered.api.proxy.Player;
import com.velocitypowered.api.proxy.ProxyServer;
import com.velocitypowered.api.plugin.Plugin;
import net.kyori.adventure.text.Component;
import org.slf4j.Logger;

/**
 * Main Velocity proxy plugin class.
 * Listens for player connections and server switches.
 */
@Plugin(
    id = "velocityplugin",
    name = "VelocityPlugin",
    version = "1.0.0",
    authors = {"ExampleAuthor"}
)
public class VelocityPlugin {

    private final ProxyServer server;
    private final Logger logger;

    @Inject
    public VelocityPlugin(ProxyServer server, Logger logger, CommandManager commandManager) {
        this.server = server;
        this.logger = logger;

        // Register the /hub command to send players to the lobby
        commandManager.register("hub", new HubCommand());
        logger.info("VelocityPlugin has been initialized!");
    }

    /**
     * Greets a player when they first connect to the proxy.
     *
     * @param event The login event with the player reference.
     */
    @Subscribe
    public void onPostLogin(PostLoginEvent event) {
        Player player = event.getPlayer();
        player.sendMessage(Component.text("Welcome to the proxy, " + player.getUsername() + "!"));
        logger.info("Player {} connected to the proxy.", player.getUsername());
    }

    /**
     * Notifies a player when they switch between backend servers.
     *
     * @param event The connect event with the connected player.
     */
    @Subscribe
    public void onServerSwitch(ServerPostConnectEvent event) {
        Player player = event.getPlayer();
        player.sendMessage(Component.text("You have been connected to a new server!"));
    }

    /**
     * Command that sends the player to the lobby server named "lobby".
     */
    private class HubCommand implements SimpleCommand {
        @Override
        public void execute(SimpleCommand.Invocation invocation) {
            CommandSource source = invocation.source();
            if (source instanceof Player player) {
                server.getServer("lobby").ifPresentOrElse(
                    serverConnection -> player.createConnectionRequest(serverConnection).connect(),
                    () -> player.sendMessage(Component.text("Lobby server not found."))
                );
            }
        }
    }
}
