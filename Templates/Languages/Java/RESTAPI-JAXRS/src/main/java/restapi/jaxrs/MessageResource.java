package restapi.jaxrs;

import jakarta.ws.rs.*;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.atomic.AtomicLong;

@Path("/messages")
@Produces(MediaType.APPLICATION_JSON)
@Consumes(MediaType.APPLICATION_JSON)
public record MessageResource() {

    private static final AtomicLong ID_GEN = new AtomicLong(1);
    private static final ConcurrentHashMap<Long, Message> STORE = new ConcurrentHashMap<>();

    @GET
    public Map<Long, Message> getAll() {
        return STORE;
    }

    @GET
    @Path("/{id}")
    public Response getById(@PathParam("id") Long id) {
        var msg = STORE.get(id);
        if (msg == null) return Response.status(404).build();
        return Response.ok(msg).build();
    }

    @POST
    public Response create(Message message) {
        var id = ID_GEN.getAndIncrement();
        var created = new Message(id, message.content());
        STORE.put(id, created);
        return Response.status(201).entity(created).build();
    }

    @DELETE
    @Path("/{id}")
    public Response delete(@PathParam("id") Long id) {
        if (STORE.remove(id) == null) return Response.status(404).build();
        return Response.noContent().build();
    }

    public record Message(Long id, String content) {}
}
