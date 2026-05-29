/**
 * Example plugin implementation demonstrating the plugin API.
 * <p>
 * This plugin simply echoes the context object as a string.
 * Developers can use this as a starting point for their own plugins.
 * The JAR manifest must contain a "Plugin-Class" attribute pointing
 * to this class.
 */
package com.example.plugin.impl;

import com.example.plugin.Plugin;

public class ExamplePlugin implements Plugin {

    @Override
    public String getName() {
        return "Example Plugin";
    }

    @Override
    public String getVersion() {
        return "1.0.0";
    }

    /**
     * Echo the context object as a formatted string.
     *
     * @param context any object the framework passes (may be null)
     * @return a string representation of the context
     */
    @Override
    public String execute(Object context) {
        return "[ExamplePlugin] Received context: " + (context != null ? context.toString() : "null");
    }
}
