/**
 * Android-specific platform implementation for the shared KMP module.
 * Returns a simple platform identifier string.
 */
package shared

/** Returns a hardcoded string identifying this platform as Android. */
actual fun getPlatformName(): String = "Android (KMP)"
