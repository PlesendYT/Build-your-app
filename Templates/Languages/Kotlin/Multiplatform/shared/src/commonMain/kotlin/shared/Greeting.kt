package shared

import kotlinx.serialization.Serializable
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.flow

@Serializable
data class GreetingData(val message: String, val platform: String)

expect fun getPlatformName(): String

class Greeting {
    fun greet(name: String): GreetingData {
        return GreetingData(
            message = "Hallo, $name!",
            platform = getPlatformName()
        )
    }

    fun greetFlow(names: List<String>): Flow<GreetingData> = flow {
        for (name in names) {
            emit(greet(name))
        }
    }
}
