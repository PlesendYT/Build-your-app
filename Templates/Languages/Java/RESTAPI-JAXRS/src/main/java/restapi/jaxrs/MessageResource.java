/**
 * JAX-RS REST API sample: in-memory CRUD for messages.
 */
package restapi.jaxrs;

import jakarta.ws.rs.*;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.atomic.AtomicLong;

/** REST resource exposing /messages endpoints backed by a thread-safe map. */
@Path("/messages")
@Produces(MediaType.APPLICATION_JSON)
@Consumes(MediaType.APPLICATION_JSON)
public record MessageResource() {

    private static final AtomicLong ID_GEN = new AtomicLong(1);
    private static final ConcurrentHashMap<Long, Message> STORE = new ConcurrentHashMap<>();

    /** Returns all stored messages. */
    @GET
    public Map<Long, Message> getAll() {
        return STORE;
    }

    /** Returns a single message by ID, or 404 if not found. */
    @GET
    @Path("/{id}")
    public Response getById(@PathParam("id") Long id) {
        var msg = STORE.get(id);
        if (msg == null) return Response.status(404).build();
        return Response.ok(msg).build();
    }

    /** Creates a new message and returns it with HTTP 201. */
    @POST
    public Response create(Message message) {
        var id = ID_GEN.getAndIncrement();
        var created = new Message(id, message.content());
        STORE.put(id, created);
        return Response.status(201).entity(created).build();
    }

    /** Deletes a message by ID; returns 404 if it does not exist. */
    @DELETE
    @Path("/{id}")
    public Response delete(@PathParam("id") Long id) {
        if (STORE.remove(id) == null) return Response.status(404).build();
        return Response.noContent().build();
    }

    /** Immutable message record with an ID and text content. */
    public record Message(Long id, String content) {}
}
