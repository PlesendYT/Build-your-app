package com.example.mod;

import net.minecraft.core.BlockPos;
import net.minecraft.network.chat.Component;
import net.minecraft.world.level.Level;
import net.minecraft.world.level.block.Blocks;
import net.minecraftforge.common.MinecraftForge;
import net.minecraftforge.event.TickEvent;
import net.minecraftforge.event.entity.player.PlayerEvent;
import net.minecraftforge.eventbus.api.IEventBus;
import net.minecraftforge.fml.common.Mod;
import net.minecraftforge.fml.javafmlmod.FMLJavaModLoadingContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

/**
 * Main Forge mod class.
 * Registers event handlers during the mod construction phase.
 */
@Mod(ForgeMod.MOD_ID)
public class ForgeMod {

    // Unique mod identifier for Forge registries
    public static final String MOD_ID = "forgemod";
    // Logger for mod-related messages
    public static final Logger LOGGER = LoggerFactory.getLogger(MOD_ID);

    public ForgeMod() {
        // Obtain the mod event bus for future registry events
        IEventBus modBus = FMLJavaModLoadingContext.get().getModEventBus();

        // Register event handlers on the Forge event bus
        MinecraftForge.EVENT_BUS.register(this);

        LOGGER.info("ForgeMod has been initialized!");
    }

    /**
     * Called when a player logs in — sends a welcome message.
     *
     * @param event The player login event.
     */
    // @SubscribeEvent is implicit when registered via EVENT_BUS.register
    public void onPlayerLogin(PlayerEvent.PlayerLoggedInEvent event) {
        event.getEntity().sendSystemMessage(Component.literal("Welcome to the Forge server!"));
        LOGGER.info("Player {} logged in.", event.getEntity().getName().getString());
    }

    /**
     * Every server tick, place a torch at the player's feet if they are in the dark.
     *
     * @param event The server tick event.
     */
    public void onServerTick(TickEvent.ServerTickEvent event) {
        // This is a placeholder demonstrating tick-based logic
        if (event.phase == TickEvent.Phase.START) return;

        Level level = net.minecraft.server.MinecraftServer.getServer().overworld();
        if (level != null && level.getGameTime() % 200 == 0) {
            // Every 10 seconds, log a heartbeat
            LOGGER.debug("Server tick heartbeat at time {}", level.getGameTime());
        }
    }
}
