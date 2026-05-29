/**
 * iOS-specific platform implementation for the shared KMP module.
 * Provides the actual iOS device name and OS version string.
 */
package shared

import platform.UIKit.UIDevice

/** Returns the iOS device model name and system version via UIDevice. */
actual fun getPlatformName(): String =
    UIDevice.currentDevice.systemName() + " " + UIDevice.currentDevice.systemVersion
