package ktorapp

import io.ktor.server.engine.*
import io.ktor.server.netty.*
import io.ktor.server.application.*
import io.ktor.server.routing.*
import io.ktor.server.response.*
import io.ktor.server.request.*
import io.ktor.serialization.kotlinx.json.*
import io.ktor.server.plugins.contentnegotiation.*
import kotlinx.serialization.Serializable
import org.jetbrains.exposed.sql.*
import org.jetbrains.exposed.sql.transactions.transaction
import org.jetbrains.exposed.dao.*
import org.jetbrains.exposed.dao.id.*

@Serializable
data class ItemResponse(val id: Int, val name: String, val value: String?)

object Items : IntIdTable() {
    val name = varchar("name", 255)
    val value = varchar("value", 255).nullable()
}

class Item(id: EntityID<Int>) : IntEntity(id) {
    companion object : IntEntityClass<Item>(Items)
    var name by Items.name
    var value by Items.value
}

fun Application.module() {
    Database.connect("jdbc:h2:mem:ktorapi", driver = "org.h2.Driver")
    transaction {
        SchemaUtils.create(Items)
        Item.new { name = "Beispiel"; value = "Initial" }
    }

    install(ContentNegotiation) {
        json()
    }

    routing {
        get("/api/items") {
            val items = transaction {
                Item.all().map { ItemResponse(it.id.value, it.name, it.value) }
            }
            call.respond(items)
        }

        get("/api/items/{id}") {
            val id = call.parameters["id"]?.toIntOrNull() ?: return@get call.respondText("Invalid ID")
            val item = transaction { Item.findById(id) }
            if (item != null) call.respond(ItemResponse(item.id.value, item.name, item.value))
            else call.respondText("Not found")
        }

        post("/api/items") {
            val data = call.receive<ItemResponse>()
            val item = transaction {
                Item.new { name = data.name; value = data.value }
            }
            call.respond(ItemResponse(item.id.value, item.name, item.value))
        }
    }
}

fun main() {
    embeddedServer(Netty, port = 8080, module = Application::module).start(wait = true)
}
