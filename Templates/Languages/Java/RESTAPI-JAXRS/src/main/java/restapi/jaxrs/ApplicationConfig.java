/**
 * JAX-RS application configuration that sets the base API path.
 */
package restapi.jaxrs;

import jakarta.ws.rs.ApplicationPath;
import jakarta.ws.rs.core.Application;

/** Registers the /api base path for all JAX-RS resources. */
@ApplicationPath("/api")
public class ApplicationConfig extends Application {
}
