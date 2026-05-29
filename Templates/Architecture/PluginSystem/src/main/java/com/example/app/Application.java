/**
 * Main application that demonstrates the plugin system.
 * <p>
 * Loads all plugins from the "plugins" directory and executes each
 * one with a sample context object. This shows how the core application
 * remains completely decoupled from plugin implementations.
 */
package com.example.app;

import com.example.plugin.Plugin;
import com.example.plugin.PluginManager;

public class Application {

    /**
     * Entry point: load plugins and execute them.
     *
     * @param args command-line arguments (unused)
     */
    public static void main(String[] args) {
        System.out.println("Plugin System Demo");
        System.out.println("=================");

        PluginManager manager = new PluginManager();
        manager.loadPlugins("plugins");

        List<Plugin> plugins = manager.getPlugins();
        if (plugins.isEmpty()) {
            System.out.println("No plugins found. Place JAR files in the 'plugins/' directory.");
            return;
        }

        System.out.println("Loaded " + plugins.size() + " plugin(s):");
        for (Plugin plugin : plugins) {
            System.out.println("  - " + plugin.getName() + " v" + plugin.getVersion());
            String result = plugin.execute("Hello from the core application!");
            System.out.println("    Result: " + result);
        }
    }
}
