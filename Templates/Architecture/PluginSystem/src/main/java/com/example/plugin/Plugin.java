/**
 * Interface that all plugins must implement.
 * <p>
 * The Plugin interface defines the contract between the application
 * core and dynamically loaded extensions. Each plugin provides a
 * name, version, and an execute method that the framework calls.
 */
package com.example.plugin;

public interface Plugin {

    /** Return the human-readable name of this plugin. */
    String getName();

    /** Return the version string of this plugin (e.g. "1.0.0"). */
    String getVersion();

    /**
     * Execute the plugin's main functionality.
     *
     * @param context a free-form context object provided by the framework
     * @return a result string that the framework can log or display
     */
    String execute(Object context);
}
