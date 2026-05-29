/**
 * Shared common code for the Kotlin Multiplatform module.
 * Defines the Greeting API that platform-specific UIs consume.
 */
package shared

import kotlinx.serialization.Serializable
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.flow

/** Serializable data class holding a greeting message and the originating platform name. */
@Serializable
data class GreetingData(val message: String, val platform: String)

/** Expect declaration — each platform must provide its own implementation. */
expect fun getPlatformName(): String

/** Provides greeting functionality backed by platform-specific implementations. */
class Greeting {
    /** Returns a single GreetingData for the given name. */
    fun greet(name: String): GreetingData {
        return GreetingData(
            message = "Hallo, $name!",
            platform = getPlatformName()
        )
    }

    /** Returns a cold Flow that emits a GreetingData for each name in the list. */
    fun greetFlow(names: List<String>): Flow<GreetingData> = flow {
        for (name in names) {
            emit(greet(name))
        }
    }
}
