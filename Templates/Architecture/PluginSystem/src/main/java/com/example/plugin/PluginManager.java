/**
 * Manages plugin discovery, loading, and lifecycle.
 * <p>
 * The PluginManager scans a designated directory for JAR files,
 * loads any classes that implement the Plugin interface, and
 * provides access to them at runtime without restarting the app.
 */
package com.example.plugin;

import java.io.File;
import java.net.URL;
import java.net.URLClassLoader;
import java.util.*;
import java.util.jar.JarFile;

public class PluginManager {

    private final List<Plugin> plugins = new ArrayList<>();

    /**
     * Load all plugins from the given directory path.
     * <p>
     * Each JAR file in the directory is inspected for classes that
     * implement the Plugin interface. Loaded plugins are cached
     * internally and returned as a list.
     *
     * @param pluginDir path to the directory containing plugin JARs
     * @return list of successfully loaded Plugin instances
     */
    public List<Plugin> loadPlugins(String pluginDir) {
        plugins.clear();
        File dir = new File(pluginDir);
        if (!dir.isDirectory()) {
            System.out.println("Plugin directory not found: " + pluginDir);
            return plugins;
        }

        File[] jars = dir.listFiles((f, name) -> name.endsWith(".jar"));
        if (jars == null) return plugins;

        for (File jar : jars) {
            try (JarFile jarFile = new JarFile(jar);
                 URLClassLoader loader = new URLClassLoader(
                     new URL[]{jar.toURI().toURL()},
                     getClass().getClassLoader())) {

                // Find the first class in the JAR that implements Plugin
                String mainClass = jarFile.getManifest()
                    .getMainAttributes().getValue("Plugin-Class");
                if (mainClass != null) {
                    Class<?> clazz = loader.loadClass(mainClass);
                    if (Plugin.class.isAssignableFrom(clazz)) {
                         plugins.add((Plugin) clazz.getDeclaredConstructor().newInstance());
                    }
                }
            } catch (Exception e) {
                System.out.println("Failed to load plugin: " + jar.getName() + " - " + e.getMessage());
            }
        }
        return plugins;
    }

    /** Return all currently loaded plugins. */
    public List<Plugin> getPlugins() {
        return Collections.unmodifiableList(plugins);
    }
}
