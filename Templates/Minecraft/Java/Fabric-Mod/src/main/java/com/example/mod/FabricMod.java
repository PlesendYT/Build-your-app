package com.example.mod;

import net.fabricmc.api.ModInitializer;
import net.fabricmc.fabric.api.event.player.PlayerBlockBreakEvents;
import net.minecraft.block.Blocks;
import net.minecraft.text.Text;
import net.minecraft.util.Identifier;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

/**
 * Main entry point for the Fabric example mod.
 * Implements ModInitializer to run logic during mod initialization.
 */
public class FabricMod implements ModInitializer {

    // Unique mod ID used for registries and resource identifiers
    public static final String MOD_ID = "fabricmod";
    // Logger instance for console output
    public static final Logger LOGGER = LoggerFactory.getLogger(MOD_ID);

    @Override
    public void onInitialize() {
        LOGGER.info("FabricMod has been initialized!");

        // Register a callback that fires when any player breaks a block
        PlayerBlockBreakEvents.AFTER.register((world, player, pos, state, entity) -> {
            // Notify the player when they break diamond ore
            if (state.getBlock() == Blocks.DIAMOND_ORE) {
                player.sendMessage(Text.literal("You broke diamond ore!"), false);
                LOGGER.info("{} broke diamond ore at {}", player.getName().getString(), pos);
            }
        });
    }
}
