package com.example.mod;

import net.minecraft.network.chat.Component;
import net.minecraft.world.level.block.Blocks;
import net.neoforged.bus.api.IEventBus;
import net.neoforged.bus.api.SubscribeEvent;
import net.neoforged.fml.common.Mod;
import net.neoforged.neoforge.event.entity.player.PlayerEvent;
import net.neoforged.neoforge.event.level.BlockEvent;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

/**
 * Main NeoForge mod class.
 * NeoForge uses a mod bus and the event bus for different lifecycle events.
 */
@Mod(NeoForgeMod.MOD_ID)
public class NeoForgeMod {

    // Unique mod identifier
    public static final String MOD_ID = "neoforgemod";
    // Logger instance
    public static final Logger LOGGER = LoggerFactory.getLogger(MOD_ID);

    public NeoForgeMod(IEventBus modBus) {
        // Register this instance on the NeoForge event bus
        modBus.addListener(this::onPlayerLogin);

        LOGGER.info("NeoForgeMod has been initialized!");
    }

    /**
     * Sends a welcome message when a player logs into the world.
     *
     * @param event The player login event.
     */
    public void onPlayerLogin(PlayerEvent.PlayerLoggedInEvent event) {
        event.getEntity().sendSystemMessage(
            Component.literal("Welcome to the NeoForge server!")
        );
        LOGGER.info("Player {} logged in.", event.getEntity().getName().getString());
    }

    /**
     * Listens for block break events and logs diamond ore breaks.
     *
     * @param event The block break event.
     */
    @SubscribeEvent
    public void onBlockBreak(BlockEvent.BreakEvent event) {
        if (event.getState().getBlock() == Blocks.DIAMOND_ORE) {
            event.getPlayer().sendSystemMessage(
                Component.literal("You broke diamond ore!")
            );
        }
    }
}
